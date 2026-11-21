using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Experiment9.Data;
using Experiment9.Models;

namespace Experiment9
{
    public partial class Placements : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlacementsGrid();
                PopulateStudentDropdown();
                PopulateCompanyDropdown();
            }
        }

        private void BindPlacementsGrid()
        {
            try
            {
                DataTable dt = PlacementDAL.GetAllPlacementsDataTable();
                gvPlacements.DataSource = dt;
                gvPlacements.DataBind();
            }
            catch (Exception ex)
            {
                ShowNotification("Error loading placements: " + ex.Message, "danger");
            }
        }

        private void PopulateStudentDropdown()
        {
            try
            {
                DataTable dt = StudentDAL.GetStudentsDataTable();
                ddlStudents.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string text = string.Format("{0} ({1} - CGPA: {2:0.00})", row["StudentName"], row["Department"], row["CGPA"]);
                    string val = row["StudentID"].ToString();
                    ddlStudents.Items.Add(new ListItem(text, val));
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error loading students: " + ex.Message, "danger");
            }
        }

        private void PopulateCompanyDropdown()
        {
            try
            {
                DataTable dt = CompanyDAL.GetAllCompaniesDataTable();
                ddlCompanies.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string text = string.Format("{0} ({1} - ₹{2:0.00} LPA)", row["CompanyName"], row["JobRole"], row["PackageLPA"]);
                    string val = row["CompanyID"].ToString();
                    ddlCompanies.Items.Add(new ListItem(text, val));
                }

                if (ddlCompanies.Items.Count > 0)
                {
                    AutoFillOfferDetails(Convert.ToInt32(ddlCompanies.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error loading companies: " + ex.Message, "danger");
            }
        }

        protected void ddlCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanies.SelectedIndex >= 0)
            {
                AutoFillOfferDetails(Convert.ToInt32(ddlCompanies.SelectedValue));
            }
        }

        private void AutoFillOfferDetails(int companyId)
        {
            try
            {
                DataTable dt = CompanyDAL.GetAllCompaniesDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["CompanyID"]) == companyId)
                    {
                        txtOfferPackage.Text = Convert.ToDecimal(row["PackageLPA"]).ToString("0.00");
                        txtOfferDesignation.Text = row["JobRole"].ToString();
                        break;
                    }
                }
            }
            catch { }
        }

        protected void btnRecordOffer_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlStudents.SelectedIndex < 0 || ddlCompanies.SelectedIndex < 0)
                {
                    ShowNotification("Please select both a student and a company.", "warning");
                    return;
                }

                int studentId = Convert.ToInt32(ddlStudents.SelectedValue);
                int companyId = Convert.ToInt32(ddlCompanies.SelectedValue);
                decimal package = Convert.ToDecimal(txtOfferPackage.Text.Trim());
                string designation = txtOfferDesignation.Text.Trim();
                string status = ddlOfferStatus.SelectedValue;

                int newId = PlacementDAL.RecordPlacement(studentId, companyId, package, designation, status);
                if (newId > 0)
                {
                    ShowNotification("Placement offer recorded successfully with ID #" + newId + "! Student status updated.", "success");
                    BindPlacementsGrid();
                    PopulateStudentDropdown();
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Offer Recording Error: " + ex.Message, "danger");
            }
        }

        protected void gvPlacements_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int placementId = Convert.ToInt32(gvPlacements.DataKeys[e.RowIndex].Value);
                bool deleted = PlacementDAL.DeletePlacement(placementId);

                if (deleted)
                {
                    ShowNotification("Placement offer #" + placementId + " deleted successfully.", "success");
                    BindPlacementsGrid();
                    PopulateStudentDropdown();
                }
                else
                {
                    ShowNotification("Failed to delete placement offer.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Delete Error: " + ex.Message, "danger");
            }
        }

        private void ShowNotification(string message, string type)
        {
            SiteMaster master = Master as SiteMaster;
            if (master != null)
            {
                master.ShowAlert(message, type);
            }
        }
    }
}


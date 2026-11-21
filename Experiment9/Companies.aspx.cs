using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Experiment9.Data;
using Experiment9.Models;

namespace Experiment9
{
    public partial class Companies : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNewVisitDate.Text = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
                BindDataList();
            }
        }

        private void BindDataList()
        {
            try
            {
                string search = txtCompanySearch.Text.Trim();
                DataTable dt = CompanyDAL.GetAllCompaniesDataTable(search);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dlCompanies.DataSource = dt;
                    dlCompanies.DataBind();
                    pnlNoCompanies.Visible = false;
                }
                else
                {
                    dlCompanies.DataSource = null;
                    dlCompanies.DataBind();
                    pnlNoCompanies.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error loading companies: " + ex.Message, "danger");
            }
        }

        protected void btnSearchCompany_Click(object sender, EventArgs e)
        {
            BindDataList();
        }

        protected void btnResetCompany_Click(object sender, EventArgs e)
        {
            txtCompanySearch.Text = string.Empty;
            BindDataList();
        }

        protected void dlCompanies_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCompany")
            {
                try
                {
                    int companyId = Convert.ToInt32(e.CommandArgument);
                    bool deleted = CompanyDAL.DeleteCompany(companyId);

                    if (deleted)
                    {
                        ShowNotification("Company removed successfully.", "success");
                        BindDataList();
                    }
                    else
                    {
                        ShowNotification("Failed to remove company.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowNotification("Delete Error: " + ex.Message, "danger");
                }
            }
        }

        protected void btnSaveCompany_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNewCompanyName.Text) || string.IsNullOrWhiteSpace(txtNewLocation.Text))
                {
                    ShowNotification("Please provide company name and location.", "warning");
                    return;
                }

                var company = new Company
                {
                    CompanyName = txtNewCompanyName.Text.Trim(),
                    Location = txtNewLocation.Text.Trim(),
                    JobRole = txtNewJobRole.Text.Trim(),
                    PackageLPA = Convert.ToDecimal(txtNewPackage.Text.Trim()),
                    EligibilityCriteria = txtNewEligibility.Text.Trim(),
                    VisitDate = Convert.ToDateTime(txtNewVisitDate.Text)
                };

                int newId = CompanyDAL.InsertCompany(company);
                if (newId > 0)
                {
                    ShowNotification("Company '" + company.CompanyName + "' registered successfully!", "success");

                    // Clear form
                    txtNewCompanyName.Text = string.Empty;
                    txtNewLocation.Text = string.Empty;
                    txtNewJobRole.Text = string.Empty;
                    txtNewPackage.Text = string.Empty;
                    txtNewEligibility.Text = string.Empty;

                    BindDataList();
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Registration Error: " + ex.Message, "danger");
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


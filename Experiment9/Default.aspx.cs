using System;
using System.Data;
using System.Web.UI;
using Experiment9.Data;
using Experiment9.Models;

namespace Experiment9
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStatistics();
                LoadRecentPlacements();
            }
        }

        private void LoadDashboardStatistics()
        {
            try
            {
                DashboardStats stats = PlacementDAL.GetDashboardStats();

                lblTotalStudents.Text = stats.TotalStudents.ToString();
                lblTotalCompanies.Text = stats.TotalCompanies.ToString();
                lblStudentsPlaced.Text = stats.StudentsPlaced.ToString();
                lblStudentsNotPlaced.Text = stats.StudentsNotPlaced.ToString();
                lblTotalOffers.Text = stats.TotalPlacementOffers.ToString();
                lblPlacementPercentage.Text = stats.PlacementPercentage.ToString("0.0") + "%";

                lblHighestPackage.Text = string.Format("₹{0:0.00} LPA", stats.HighestPackageLPA);
                lblAveragePackage.Text = string.Format("₹{0:0.00} LPA", stats.AveragePackageLPA);

                lblProgressText.Text = stats.PlacementPercentage.ToString("0.0") + "% Placed";
                divProgressBar.Attributes["style"] = "width: " + stats.PlacementPercentage.ToString("0.0") + "%;";
            }
            catch (Exception ex)
            {
                SiteMaster master = Master as SiteMaster;
                if (master != null)
                {
                    master.ShowAlert("Error loading dashboard metrics: " + ex.Message, "danger");
                }
            }
        }

        private void LoadRecentPlacements()
        {
            try
            {
                DataTable dt = PlacementDAL.GetRecentPlacementsDataTable(6);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptRecentPlacements.DataSource = dt;
                    rptRecentPlacements.DataBind();
                    pnlNoOffers.Visible = false;
                }
                else
                {
                    rptRecentPlacements.DataSource = null;
                    rptRecentPlacements.DataBind();
                    pnlNoOffers.Visible = true;
                }
            }
            catch (Exception ex)
            {
                SiteMaster master = Master as SiteMaster;
                if (master != null)
                {
                    master.ShowAlert("Error loading recent activities: " + ex.Message, "danger");
                }
            }
        }
    }
}


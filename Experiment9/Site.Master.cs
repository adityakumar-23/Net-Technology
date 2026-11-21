using System;
using System.Web.UI;
using Experiment9.Data;

namespace Experiment9
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            // Auto-initialize PlacementDB on first application hit
            DatabaseHelper.InitializeDatabase();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void ShowAlert(string message, string type = "success")
        {
            pnlAlert.Visible = true;
            lblAlertMessage.Text = message;

            if (type.ToLower() == "success")
            {
                divAlertBox.Attributes["class"] = "alert alert-success alert-dismissible fade show shadow-sm border-0 d-flex align-items-center mb-0";
                iconAlert.Attributes["class"] = "bi bi-check-circle-fill fs-5 me-2 text-success";
            }
            else if (type.ToLower() == "danger" || type.ToLower() == "error")
            {
                divAlertBox.Attributes["class"] = "alert alert-danger alert-dismissible fade show shadow-sm border-0 d-flex align-items-center mb-0";
                iconAlert.Attributes["class"] = "bi bi-exclamation-octagon-fill fs-5 me-2 text-danger";
            }
            else if (type.ToLower() == "warning")
            {
                divAlertBox.Attributes["class"] = "alert alert-warning alert-dismissible fade show shadow-sm border-0 d-flex align-items-center mb-0";
                iconAlert.Attributes["class"] = "bi bi-exclamation-triangle-fill fs-5 me-2 text-warning";
            }
            else
            {
                divAlertBox.Attributes["class"] = "alert alert-info alert-dismissible fade show shadow-sm border-0 d-flex align-items-center mb-0";
                iconAlert.Attributes["class"] = "bi bi-info-circle-fill fs-5 me-2 text-info";
            }
        }
    }
}


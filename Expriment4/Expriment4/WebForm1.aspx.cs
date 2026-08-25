using System;
using System.Web.UI;

namespace EventPortal
{
    public partial class StudentRegister : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load initialization
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string rollNo = txtRollNo.Text.Trim();
                string studentName = txtName.Text.Trim();
                string branch = ddlBranch.SelectedValue;
                string email = txtEmail.Text.Trim();
                string mobile = txtMobile.Text.Trim();
                string selectedEvent = ddlEvent.SelectedItem.Text;

                // Success Message display
                lblMessage.Text = $"<b>Registration Successful!</b><br />" +
                                  $"Name: {studentName} (Roll No: {rollNo})<br />" +
                                  $"Branch: {branch} | Phone: {mobile}<br />" +
                                  $"Event Registered: <b>{selectedEvent}</b><br />" +
                                  $"Confirmation email sent to {email}.";

                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtRollNo.Text = string.Empty;
            txtName.Text = string.Empty;
            ddlBranch.SelectedIndex = 0;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;
            ddlEvent.SelectedIndex = 0;
        }
    }
}
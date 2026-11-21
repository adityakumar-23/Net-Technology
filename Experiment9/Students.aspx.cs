using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Experiment9.Data;
using Experiment9.Models;

namespace Experiment9
{
    public partial class Students : Page
    {
        private string CurrentSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? "StudentID"; }
            set { ViewState["SortExpression"] = value; }
        }

        private string CurrentSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartmentFilter();
                BindGridView();
                BindDetailsView();
            }
        }

        private void BindDepartmentFilter()
        {
            try
            {
                var depts = StudentDAL.GetDepartments();
                ddlDepartmentFilter.Items.Clear();
                ddlDepartmentFilter.Items.Add(new ListItem("All Departments", "All"));
                foreach (var dept in depts)
                {
                    ddlDepartmentFilter.Items.Add(new ListItem(dept, dept));
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error loading departments: " + ex.Message, "danger");
            }
        }

        // =====================================================================
        // 1. GRIDVIEW DATA BINDING & EVENT HANDLERS
        // =====================================================================
        private void BindGridView()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                string dept = ddlDepartmentFilter.SelectedValue;

                DataTable dt = StudentDAL.GetStudentsDataTable(search, dept, CurrentSortExpression, CurrentSortDirection);
                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
            catch (Exception ex)
            {
                ShowNotification("GridView DataBinding Error: " + ex.Message, "danger");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvStudents.PageIndex = 0;
            BindGridView();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlDepartmentFilter.SelectedValue = "All";
            gvStudents.PageIndex = 0;
            BindGridView();
        }

        protected void ddlDepartmentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStudents.PageIndex = 0;
            BindGridView();
        }

        // Paging Event
        protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStudents.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        // Sorting Event
        protected void gvStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (CurrentSortExpression == e.SortExpression)
            {
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }

            BindGridView();
        }

        // Edit Mode Events
        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            BindGridView();

            // Set selected dropdown values in EditItemTemplate
            GridViewRow row = gvStudents.Rows[e.NewEditIndex];
            DataTable dt = StudentDAL.GetStudentsDataTable(txtSearch.Text.Trim(), ddlDepartmentFilter.SelectedValue, CurrentSortExpression, CurrentSortDirection);
            if (dt != null && dt.Rows.Count > (gvStudents.PageIndex * gvStudents.PageSize + e.NewEditIndex))
            {
                DataRow dr = dt.Rows[gvStudents.PageIndex * gvStudents.PageSize + e.NewEditIndex];
                
                DropDownList ddlDept = row.FindControl("ddlEditDept") as DropDownList;
                if (ddlDept != null)
                {
                    ddlDept.SelectedValue = dr["Department"].ToString();
                }

                DropDownList ddlStatus = row.FindControl("ddlEditStatus") as DropDownList;
                if (ddlStatus != null)
                {
                    ddlStatus.SelectedValue = dr["PlacementStatus"].ToString();
                }
            }
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            BindGridView();
        }

        // Update Event
        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvStudents.Rows[e.RowIndex];

                TextBox txtName = row.FindControl("txtEditName") as TextBox;
                TextBox txtEmail = row.FindControl("txtEditEmail") as TextBox;
                DropDownList ddlDept = row.FindControl("ddlEditDept") as DropDownList;
                TextBox txtSem = row.FindControl("txtEditSem") as TextBox;
                TextBox txtCGPA = row.FindControl("txtEditCGPA") as TextBox;
                TextBox txtSkills = row.FindControl("txtEditSkills") as TextBox;
                DropDownList ddlStatus = row.FindControl("ddlEditStatus") as DropDownList;

                var student = new Student
                {
                    StudentID = studentId,
                    StudentName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Department = ddlDept.SelectedValue,
                    Semester = Convert.ToInt32(txtSem.Text.Trim()),
                    CGPA = Convert.ToDecimal(txtCGPA.Text.Trim()),
                    Skills = txtSkills.Text.Trim(),
                    PlacementStatus = ddlStatus.SelectedValue
                };

                bool updated = StudentDAL.UpdateStudent(student);
                if (updated)
                {
                    gvStudents.EditIndex = -1;
                    BindGridView();
                    BindDetailsView();
                    ShowNotification("Student record (ID #" + studentId + ") updated successfully!", "success");
                }
                else
                {
                    ShowNotification("Failed to update student record.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Update Error: " + ex.Message, "danger");
            }
        }

        // Delete Event
        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                bool deleted = StudentDAL.DeleteStudent(studentId);

                if (deleted)
                {
                    BindGridView();
                    BindDetailsView();
                    ShowNotification("Student record (ID #" + studentId + ") deleted successfully.", "success");
                }
                else
                {
                    ShowNotification("Could not delete student record.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Delete Error: " + ex.Message, "danger");
            }
        }

        // =====================================================================
        // 2. DETAILSVIEW DATA BINDING & EVENT HANDLERS
        // =====================================================================
        private void BindDetailsView()
        {
            try
            {
                DataTable dt = StudentDAL.GetStudentsDataTable(null, null, "StudentID", "ASC");
                dvStudent.DataSource = dt;
                dvStudent.DataBind();
            }
            catch (Exception ex)
            {
                ShowNotification("DetailsView Error: " + ex.Message, "danger");
            }
        }

        protected void dvStudent_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            dvStudent.PageIndex = e.NewPageIndex;
            BindDetailsView();
        }

        // =====================================================================
        // 3. FORMVIEW DATA BINDING & EVENT HANDLERS
        // =====================================================================
        protected void fvStudent_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            try
            {
                TextBox txtName = fvStudent.FindControl("txtInsertName") as TextBox;
                TextBox txtEmail = fvStudent.FindControl("txtInsertEmail") as TextBox;
                DropDownList ddlDept = fvStudent.FindControl("ddlInsertDept") as DropDownList;
                TextBox txtSem = fvStudent.FindControl("txtInsertSem") as TextBox;
                TextBox txtCGPA = fvStudent.FindControl("txtInsertCGPA") as TextBox;
                TextBox txtSkills = fvStudent.FindControl("txtInsertSkills") as TextBox;
                RadioButtonList rblStatus = fvStudent.FindControl("rblInsertStatus") as RadioButtonList;

                var student = new Student
                {
                    StudentName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Department = ddlDept.SelectedValue,
                    Semester = Convert.ToInt32(txtSem.Text.Trim()),
                    CGPA = Convert.ToDecimal(txtCGPA.Text.Trim()),
                    Skills = txtSkills.Text.Trim(),
                    PlacementStatus = rblStatus != null ? rblStatus.SelectedValue : "Not Placed"
                };

                int newId = StudentDAL.InsertStudent(student);
                if (newId > 0)
                {
                    ShowNotification("New student '" + student.StudentName + "' enrolled with ID #" + newId + "!", "success");

                    // Clear fields
                    txtName.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtSkills.Text = string.Empty;

                    // Rebind GridView and DetailsView
                    BindGridView();
                    BindDetailsView();
                    BindDepartmentFilter();
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Insert Error: " + ex.Message, "danger");
                e.Cancel = true;
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


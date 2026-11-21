<%@ Page Title="Student Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="Experiment9.Students" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3 mb-4">
        <div>
            <h2 class="fw-bold tracking-tight text-dark mb-1">Student Placement Directory</h2>
            <p class="text-muted mb-0 small">Demonstrating <strong>ASP.NET GridView</strong>, <strong>DetailsView</strong>, and <strong>FormView</strong> with ADO.NET and SQL Server.</p>
        </div>
        <div class="d-flex align-items-center gap-2">
            <span class="badge bg-primary-subtle text-primary border border-primary-subtle px-3 py-2 font-monospace">
                <i class="bi bi-database me-1"></i> PlacementDB.Students
            </span>
        </div>
    </div>

    <!-- Navigation Tabs for the 3 Controls -->
    <ul class="nav nav-tabs mb-4 border-bottom" id="studentTabs" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active fw-semibold d-flex align-items-center gap-2" id="gridview-tab" data-bs-toggle="tab" data-bs-target="#tabGridView" type="button" role="tab">
                <i class="bi bi-table text-primary"></i> 1. GridView (Paging &bull; Sorting &bull; CRUD)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold d-flex align-items-center gap-2" id="detailsview-tab" data-bs-toggle="tab" data-bs-target="#tabDetailsView" type="button" role="tab">
                <i class="bi bi-card-heading text-info"></i> 2. DetailsView (Record Inspection)
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link fw-semibold d-flex align-items-center gap-2" id="formview-tab" data-bs-toggle="tab" data-bs-target="#tabFormView" type="button" role="tab">
                <i class="bi bi-input-cursor text-success"></i> 3. FormView (Insert Record)
            </button>
        </li>
    </ul>

    <!-- Tab Content -->
    <div class="tab-content" id="studentTabsContent">
        
        <!-- =================================================================== -->
        <!-- TAB 1: ASP.NET GRIDVIEW CONTROL                                     -->
        <!-- =================================================================== -->
        <div class="tab-pane fade show active" id="tabGridView" role="tabpanel">
            <!-- Filter Toolbar -->
            <div class="card border-0 shadow-sm rounded-3 mb-3">
                <div class="card-body p-3">
                    <div class="row g-2 align-items-center">
                        <div class="col-12 col-md-5">
                            <div class="input-group input-group-sm">
                                <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm border-start-0" placeholder="Search by name, email, skills..."></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-12 col-sm-6 col-md-4">
                            <asp:DropDownList ID="ddlDepartmentFilter" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All Departments" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-12 col-sm-6 col-md-3 d-flex gap-2">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary btn-sm flex-grow-1" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-light btn-sm border" OnClick="btnReset_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- GridView Card Container -->
            <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
                <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center border-bottom">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-grid-3x3-gap-fill text-primary"></i>
                        <h6 class="mb-0 fw-bold text-dark">GridView Demonstration</h6>
                    </div>
                    <small class="text-muted">Click column headers to sort &bull; Click page numbers below to navigate</small>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <!-- 1. ASP.NET GRIDVIEW CONTROL -->
                        <asp:GridView ID="gvStudents" runat="server"
                            CssClass="table table-hover align-middle gridview-table"
                            DataKeyNames="StudentID"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            PageSize="5"
                            AllowSorting="True"
                            OnPageIndexChanging="gvStudents_PageIndexChanging"
                            OnSorting="gvStudents_Sorting"
                            OnRowEditing="gvStudents_RowEditing"
                            OnRowUpdating="gvStudents_RowUpdating"
                            OnRowCancelingEdit="gvStudents_RowCancelingEdit"
                            OnRowDeleting="gvStudents_RowDeleting"
                            GridLines="None">

                            <PagerStyle CssClass="gridview-pager" HorizontalAlign="Center" />

                            <Columns>
                                <%-- 1. Student ID --%>
                                <asp:BoundField DataField="StudentID" HeaderText="ID" ReadOnly="True" SortExpression="StudentID" ItemStyle-CssClass="ps-4 font-monospace text-muted small" HeaderStyle-CssClass="ps-4" />

                                <%-- 2. Student Name --%>
                                <asp:TemplateField HeaderText="Student Name" SortExpression="StudentName">
                                    <ItemTemplate>
                                        <span class="fw-semibold text-dark"><%# Eval("StudentName") %></span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("StudentName") %>' CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 3. Email --%>
                                <asp:TemplateField HeaderText="Email" SortExpression="Email">
                                    <ItemTemplate>
                                        <span class="text-muted small"><%# Eval("Email") %></span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 4. Department --%>
                                <asp:TemplateField HeaderText="Department" SortExpression="Department">
                                    <ItemTemplate>
                                        <span class="badge bg-light text-dark border"><%# Eval("Department") %></span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditDept" runat="server" CssClass="form-select form-select-sm">
                                            <asp:ListItem Text="Computer Engineering" Value="Computer Engineering"></asp:ListItem>
                                            <asp:ListItem Text="Information Technology" Value="Information Technology"></asp:ListItem>
                                            <asp:ListItem Text="AI & Data Science" Value="AI & Data Science"></asp:ListItem>
                                            <asp:ListItem Text="Electronics & Communication" Value="Electronics & Communication"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 5. Sem --%>
                                <asp:TemplateField HeaderText="Sem" SortExpression="Semester">
                                    <ItemTemplate>
                                        <span class="small">Sem <%# Eval("Semester") %></span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditSem" runat="server" Text='<%# Bind("Semester") %>' TextMode="Number" min="1" max="8" CssClass="form-control form-control-sm" style="width: 70px;"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 6. CGPA --%>
                                <asp:TemplateField HeaderText="CGPA" SortExpression="CGPA">
                                    <ItemTemplate>
                                        <span class="badge bg-info-subtle text-info-emphasis border border-info-subtle font-monospace"><%# string.Format("{0:0.00}", Eval("CGPA")) %></span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditCGPA" runat="server" Text='<%# Bind("CGPA") %>' CssClass="form-control form-control-sm" style="width: 80px;"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 7. Skills --%>
                                <asp:TemplateField HeaderText="Key Skills">
                                    <ItemTemplate>
                                        <small class="text-muted text-truncate d-inline-block" style="max-width: 180px;"><%# Eval("Skills") %></small>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditSkills" runat="server" Text='<%# Bind("Skills") %>' CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 8. Placement Status --%>
                                <asp:TemplateField HeaderText="Status" SortExpression="PlacementStatus">
                                    <ItemTemplate>
                                        <span class='<%# Eval("PlacementStatus").ToString() == "Placed" ? "badge bg-success" : "badge bg-secondary" %>'>
                                            <%# Eval("PlacementStatus") %>
                                        </span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-select form-select-sm">
                                            <asp:ListItem Text="Not Placed" Value="Not Placed"></asp:ListItem>
                                            <asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- 9. Commands (Edit / Delete) --%>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4" HeaderStyle-CssClass="text-end pe-4">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-outline-primary" title="Edit Inline">
                                                <i class="bi bi-pencil"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-outline-danger" title="Delete Student" OnClientClick="return confirm('Are you sure you want to permanently delete this student record?');">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-success" title="Save Changes">
                                                <i class="bi bi-check-lg"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-light border" title="Cancel Edit">
                                                <i class="bi bi-x-lg"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <div class="text-center py-5">
                                    <i class="bi bi-search text-muted fs-1 mb-2 d-block"></i>
                                    <h6 class="text-dark fw-bold">No Students Found</h6>
                                    <p class="text-muted small">No student records match your query.</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <!-- =================================================================== -->
        <!-- TAB 2: ASP.NET DETAILSVIEW CONTROL                                  -->
        <!-- =================================================================== -->
        <div class="tab-pane fade" id="tabDetailsView" role="tabpanel">
            <div class="row justify-content-center">
                <div class="col-12 col-md-8 col-lg-7">
                    <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
                        <div class="card-header bg-white py-3 px-4 border-bottom d-flex justify-content-between align-items-center">
                            <div class="d-flex align-items-center gap-2">
                                <i class="bi bi-card-heading text-info fs-5"></i>
                                <h6 class="mb-0 fw-bold text-dark">DetailsView Demonstration</h6>
                            </div>
                            <small class="text-muted">Use pagination numbers below to browse students</small>
                        </div>
                        <div class="card-body p-4">
                            <!-- 2. ASP.NET DETAILSVIEW CONTROL -->
                            <asp:DetailsView ID="dvStudent" runat="server"
                                CssClass="table table-bordered detailsview-table mb-0"
                                AutoGenerateRows="False"
                                AllowPaging="True"
                                OnPageIndexChanging="dvStudent_PageIndexChanging"
                                GridLines="None">

                                <PagerStyle CssClass="detailsview-pager text-center" />

                                <Fields>
                                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" HeaderStyle-CssClass="bg-light fw-semibold text-muted" ItemStyle-CssClass="fw-bold text-dark" />
                                    <asp:BoundField DataField="Email" HeaderText="Institutional Email" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                    <asp:BoundField DataField="Department" HeaderText="Department / Branch" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                    <asp:BoundField DataField="Semester" HeaderText="Current Semester" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                    <asp:BoundField DataField="CGPA" HeaderText="Cumulative CGPA" HeaderStyle-CssClass="bg-light fw-semibold text-muted" ItemStyle-CssClass="font-monospace fw-bold text-primary" />
                                    <asp:BoundField DataField="Skills" HeaderText="Technical Skills" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                    
                                    <asp:TemplateField HeaderText="Placement Status" HeaderStyle-CssClass="bg-light fw-semibold text-muted">
                                        <ItemTemplate>
                                            <span class='<%# Eval("PlacementStatus").ToString() == "Placed" ? "badge bg-success fs-6" : "badge bg-secondary fs-6" %>'>
                                                <%# Eval("PlacementStatus") %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="CreatedDate" HeaderText="Registered On" DataFormatString="{0:dd MMMM yyyy, hh:mm tt}" HeaderStyle-CssClass="bg-light fw-semibold text-muted" />
                                </Fields>
                            </asp:DetailsView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- =================================================================== -->
        <!-- TAB 3: ASP.NET FORMVIEW CONTROL                                     -->
        <!-- =================================================================== -->
        <div class="tab-pane fade" id="tabFormView" role="tabpanel">
            <div class="row justify-content-center">
                <div class="col-12 col-md-8 col-lg-7">
                    <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
                        <div class="card-header bg-white py-3 px-4 border-bottom d-flex justify-content-between align-items-center">
                            <div class="d-flex align-items-center gap-2">
                                <i class="bi bi-input-cursor text-success fs-5"></i>
                                <h6 class="mb-0 fw-bold text-dark">FormView Demonstration (Insert Student)</h6>
                            </div>
                            <span class="badge bg-success-subtle text-success border border-success-subtle">Insert Mode</span>
                        </div>
                        <div class="card-body p-4">
                            <!-- 3. ASP.NET FORMVIEW CONTROL -->
                            <asp:FormView ID="fvStudent" runat="server"
                                DefaultMode="Insert"
                                RenderOuterTable="false"
                                OnItemInserting="fvStudent_ItemInserting">

                                <InsertItemTemplate>
                                    <div class="row g-3">
                                        <!-- Student Name -->
                                        <div class="col-12">
                                            <label class="form-label fw-semibold small text-dark">Student Full Name <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="form-control" placeholder="e.g. Aditya Kumar"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtInsertName" ErrorMessage="Student name is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="vgInsert"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Email -->
                                        <div class="col-12 col-md-6">
                                            <label class="form-label fw-semibold small text-dark">Email Address <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtInsertEmail" runat="server" CssClass="form-control" placeholder="student@marwadi.edu"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtInsertEmail" ErrorMessage="Email is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="vgInsert"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Department -->
                                        <div class="col-12 col-md-6">
                                            <label class="form-label fw-semibold small text-dark">Department <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlInsertDept" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="Computer Engineering" Value="Computer Engineering"></asp:ListItem>
                                                <asp:ListItem Text="Information Technology" Value="Information Technology"></asp:ListItem>
                                                <asp:ListItem Text="AI & Data Science" Value="AI & Data Science"></asp:ListItem>
                                                <asp:ListItem Text="Electronics & Communication" Value="Electronics & Communication"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <!-- Semester -->
                                        <div class="col-12 col-md-6">
                                            <label class="form-label fw-semibold small text-dark">Semester (1 - 8) <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtInsertSem" runat="server" Text="5" TextMode="Number" min="1" max="8" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvSem" runat="server" ControlToValidate="txtInsertSem" ErrorMessage="Semester is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="vgInsert"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- CGPA -->
                                        <div class="col-12 col-md-6">
                                            <label class="form-label fw-semibold small text-dark">CGPA (0.00 - 10.00) <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtInsertCGPA" runat="server" Text="8.50" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCGPA" runat="server" ControlToValidate="txtInsertCGPA" ErrorMessage="CGPA is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="vgInsert"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Skills -->
                                        <div class="col-12">
                                            <label class="form-label fw-semibold small text-dark">Technical Skills</label>
                                            <asp:TextBox ID="txtInsertSkills" runat="server" CssClass="form-control" placeholder="e.g. C#, ASP.NET, SQL Server, Python, Docker"></asp:TextBox>
                                        </div>

                                        <!-- Status -->
                                        <div class="col-12">
                                            <label class="form-label fw-semibold small text-dark">Placement Status</label>
                                            <asp:RadioButtonList ID="rblInsertStatus" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-3">
                                                <asp:ListItem Text="Not Placed" Value="Not Placed" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                        <!-- Buttons -->
                                        <div class="col-12 pt-3 d-flex justify-content-end gap-2 border-top">
                                            <asp:Button ID="btnInsertStudent" runat="server" Text="Add Student Record" CommandName="Insert" ValidationGroup="vgInsert" CssClass="btn btn-success px-4 shadow-sm" />
                                        </div>
                                    </div>
                                </InsertItemTemplate>
                            </asp:FormView>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>


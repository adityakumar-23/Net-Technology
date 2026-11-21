<%@ Page Title="Corporate Recruiters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="Experiment9.Companies" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3 mb-4">
        <div>
            <h2 class="fw-bold tracking-tight text-dark mb-1">Partner Companies &amp; Recruiters</h2>
            <p class="text-muted mb-0 small">Demonstrating <strong>ASP.NET DataList Control</strong> for card-based catalog rendering.</p>
        </div>
        <div class="d-flex align-items-center gap-2">
            <button type="button" class="btn btn-primary btn-sm rounded-pill px-3 shadow-xs" data-bs-toggle="modal" data-bs-target="#addCompanyModal">
                <i class="bi bi-plus-lg me-1"></i> Add Recruiter
            </button>
        </div>
    </div>

    <!-- Search Controls -->
    <div class="card border-0 shadow-sm rounded-3 mb-4">
        <div class="card-body p-3">
            <div class="row g-2 align-items-center">
                <div class="col-12 col-md-6">
                    <div class="input-group input-group-sm">
                        <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                        <asp:TextBox ID="txtCompanySearch" runat="server" CssClass="form-control form-control-sm border-start-0" placeholder="Search by company name, location, job role..."></asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-md-3 d-flex gap-2">
                    <asp:Button ID="btnSearchCompany" runat="server" Text="Search" CssClass="btn btn-primary btn-sm flex-grow-1" OnClick="btnSearchCompany_Click" />
                    <asp:Button ID="btnResetCompany" runat="server" Text="Reset" CssClass="btn btn-light btn-sm border" OnClick="btnResetCompany_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- =================================================================== -->
    <!-- 5. ASP.NET DATALIST CONTROL IMPLEMENTATION                          -->
    <!-- =================================================================== -->
    <div class="card border-0 shadow-sm rounded-3 p-4">
        <div class="d-flex align-items-center justify-content-between mb-3 border-bottom pb-2">
            <div class="d-flex align-items-center gap-2">
                <div class="bg-danger-subtle text-danger rounded-2 p-2 d-flex align-items-center justify-content-center">
                    <i class="bi bi-grid-fill fs-5"></i>
                </div>
                <div>
                    <h5 class="mb-0 fw-bold text-dark">Recruiter Company Showcase (DataList)</h5>
                    <small class="text-muted">Cards rendered dynamically via <strong>&lt;asp:DataList&gt;</strong></small>
                </div>
            </div>
        </div>

        <asp:DataList ID="dlCompanies" runat="server"
            RepeatColumns="3"
            RepeatDirection="Horizontal"
            RepeatLayout="Flow"
            OnItemCommand="dlCompanies_ItemCommand"
            DataKeyField="CompanyID">

            <ItemTemplate>
                <div class="col-12 col-md-6 col-lg-4 p-2">
                    <div class="card company-card rounded-3 h-100 shadow-xs overflow-hidden">
                        <div class="card-body p-4 d-flex flex-column justify-content-between">
                            <div>
                                <div class="d-flex justify-content-between align-items-start mb-2">
                                    <div class="avatar bg-primary-subtle text-primary rounded-3 p-2 d-flex align-items-center justify-content-center" style="width: 44px; height: 44px;">
                                        <i class="bi bi-buildings fs-4"></i>
                                    </div>
                                    <span class="badge bg-success-subtle text-success border border-success-subtle font-monospace px-2 py-1 fs-6">
                                        ₹<%# string.Format("{0:0.00}", Eval("PackageLPA")) %> LPA
                                    </span>
                                </div>

                                <h5 class="fw-bold text-dark mb-1"><%# Eval("CompanyName") %></h5>
                                <span class="badge bg-light text-dark border mb-3"><i class="bi bi-geo-alt me-1 text-danger"></i> <%# Eval("Location") %></span>

                                <div class="mb-3">
                                    <small class="text-muted d-block text-uppercase fw-semibold" style="font-size: 0.68rem;">Hiring Profile</small>
                                    <span class="fw-semibold text-primary"><%# Eval("JobRole") %></span>
                                </div>

                                <div class="mb-3 bg-light p-2 rounded-2 small text-muted">
                                    <i class="bi bi-check2-circle text-success me-1"></i>
                                    <strong>Eligibility:</strong> <%# Eval("EligibilityCriteria") %>
                                </div>
                            </div>

                            <div class="pt-3 border-top d-flex justify-content-between align-items-center mt-auto">
                                <small class="text-muted">
                                    <i class="bi bi-calendar-event me-1"></i> Visit: <%# Convert.ToDateTime(Eval("VisitDate")).ToString("dd MMM yyyy") %>
                                </small>
                                <asp:LinkButton ID="btnDeleteCompany" runat="server" CommandName="DeleteCompany" CommandArgument='<%# Eval("CompanyID") %>' CssClass="btn btn-outline-danger btn-sm" OnClientClick="return confirm('Delete this company?');" title="Remove Company">
                                    <i class="bi bi-trash"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>

        <asp:Panel ID="pnlNoCompanies" runat="server" Visible="false" CssClass="text-center py-5">
            <i class="bi bi-building-exclamation text-muted fs-1 mb-2 d-block"></i>
            <h6 class="text-dark fw-bold">No Companies Found</h6>
            <p class="text-muted small">No corporate recruiters match your search terms.</p>
        </asp:Panel>
    </div>

    <!-- Modal for Adding New Company -->
    <div class="modal fade" id="addCompanyModal" tabindex="-1" aria-labelledby="addCompanyModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="addCompanyModalLabel"><i class="bi bi-building-add me-2 text-primary"></i> Register New Recruiter</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Company Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNewCompanyName" runat="server" CssClass="form-control" placeholder="e.g. Google India / Microsoft"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Work Location <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNewLocation" runat="server" CssClass="form-control" placeholder="e.g. Bengaluru, Hyderabad, Pune"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Job Role / Designation <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNewJobRole" runat="server" CssClass="form-control" placeholder="e.g. Software Development Engineer"></asp:TextBox>
                    </div>
                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label fw-semibold small text-dark">CTC Package (LPA) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNewPackage" runat="server" CssClass="form-control" placeholder="e.g. 12.50"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label fw-semibold small text-dark">Drive Visit Date <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNewVisitDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Eligibility Criteria</label>
                        <asp:TextBox ID="txtNewEligibility" runat="server" CssClass="form-control" placeholder="e.g. CGPA >= 7.5, No backlogs"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-light border" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnSaveCompany" runat="server" Text="Save Recruiter" CssClass="btn btn-primary" OnClick="btnSaveCompany_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>


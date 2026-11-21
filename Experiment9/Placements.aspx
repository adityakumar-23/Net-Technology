<%@ Page Title="Placement Offers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Placements.aspx.cs" Inherits="Experiment9.Placements" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3 mb-4">
        <div>
            <h2 class="fw-bold tracking-tight text-dark mb-1">Campus Placement Offers</h2>
            <p class="text-muted mb-0 small">Manage student recruitment selections, offer letters, and package details.</p>
        </div>
        <div class="d-flex align-items-center gap-2">
            <button type="button" class="btn btn-primary btn-sm rounded-pill px-3 shadow-xs" data-bs-toggle="modal" data-bs-target="#recordOfferModal">
                <i class="bi bi-award me-1"></i> Record New Offer
            </button>
        </div>
    </div>

    <!-- Placements Table Card -->
    <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
        <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center border-bottom">
            <div class="d-flex align-items-center gap-2">
                <i class="bi bi-file-earmark-check-fill text-success fs-5"></i>
                <h6 class="mb-0 fw-bold text-dark">Confirmed Placement Records</h6>
            </div>
            <span class="badge bg-secondary-subtle text-secondary font-monospace">PlacementDB.Placements</span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvPlacements" runat="server"
                    CssClass="table table-hover align-middle gridview-table"
                    AutoGenerateColumns="False"
                    DataKeyNames="PlacementID"
                    OnRowDeleting="gvPlacements_RowDeleting"
                    GridLines="None">

                    <Columns>
                        <asp:BoundField DataField="PlacementID" HeaderText="Offer ID" ReadOnly="True" ItemStyle-CssClass="ps-4 font-monospace text-muted small" HeaderStyle-CssClass="ps-4" />

                        <asp:TemplateField HeaderText="Selected Student">
                            <ItemTemplate>
                                <span class="fw-semibold text-dark d-block"><%# Eval("StudentName") %></span>
                                <small class="text-muted">Student ID: #<%# Eval("StudentID") %></small>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Department" HeaderText="Department" />

                        <asp:TemplateField HeaderText="Recruiter Company">
                            <ItemTemplate>
                                <span class="fw-semibold text-primary"><%# Eval("CompanyName") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Designation" HeaderText="Designation" />

                        <asp:TemplateField HeaderText="Offered CTC">
                            <ItemTemplate>
                                <span class="badge bg-success-subtle text-success border border-success-subtle font-monospace px-2 py-1 fs-6">
                                    ₹<%# string.Format("{0:0.00}", Eval("PackageOffered")) %> LPA
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="OfferDate" HeaderText="Offer Date" DataFormatString="{0:dd MMM yyyy}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# Eval("Status").ToString() == "Accepted" ? "badge bg-success" : "badge bg-warning text-dark" %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="text-end pe-4" HeaderStyle-CssClass="text-end pe-4">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteOffer" runat="server" CommandName="Delete" CssClass="btn btn-outline-danger btn-sm" OnClientClick="return confirm('Cancel and delete this placement offer?');" title="Delete Offer">
                                    <i class="bi bi-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="bi bi-inbox text-muted fs-1 mb-2 d-block"></i>
                            <h6 class="text-dark fw-bold">No Placement Records</h6>
                            <p class="text-muted small">No student placement offers have been recorded yet.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- Modal to Record Placement Offer -->
    <div class="modal fade" id="recordOfferModal" tabindex="-1" aria-labelledby="recordOfferModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="recordOfferModalLabel"><i class="bi bi-award-fill text-warning me-2"></i> Issue Placement Letter</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Select Student <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlStudents" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Select Recruiter Company <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlCompanies" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanies_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label fw-semibold small text-dark">CTC Package (LPA) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtOfferPackage" runat="server" CssClass="form-control" placeholder="e.g. 9.50"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label fw-semibold small text-dark">Designation <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtOfferDesignation" runat="server" CssClass="form-control" placeholder="e.g. Associate SDE"></asp:TextBox>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-semibold small text-dark">Offer Status</label>
                        <asp:DropDownList ID="ddlOfferStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Accepted" Value="Accepted" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-light border" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnRecordOffer" runat="server" Text="Confirm Placement Offer" CssClass="btn btn-success" OnClick="btnRecordOffer_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>


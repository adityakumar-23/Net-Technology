<%@ Page Title="Placement Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Experiment9.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Dashboard Header -->
    <div class="dashboard-header mb-4 d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <h2 class="fw-bold tracking-tight text-dark mb-1">Campus Placement Analytics</h2>
            <p class="text-muted mb-0 small">Real-time recruitment statistics and offer activities powered by ASP.NET Data Controls &amp; SQL Server.</p>
        </div>
        <div class="d-flex align-items-center gap-2">
            <a href="Students.aspx" class="btn btn-outline-secondary btn-sm rounded-pill px-3 shadow-xs">
                <i class="bi bi-people me-1"></i> Students Directory
            </a>
            <a href="Companies.aspx" class="btn btn-primary btn-sm rounded-pill px-3 shadow-xs">
                <i class="bi bi-buildings me-1"></i> Partner Companies
            </a>
        </div>
    </div>

    <!-- 6 KPI Metric Cards Grid -->
    <div class="row g-3 mb-4">
        <!-- 1. Total Students -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Registered</span>
                        <div class="metric-icon-wrapper bg-primary-subtle text-primary rounded-3 p-1" style="width: 32px; height: 32px;">
                            <i class="bi bi-people-fill"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-dark mb-0"><asp:Label ID="lblTotalStudents" runat="server">0</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">Eligible Students</small>
                </div>
            </div>
        </div>

        <!-- 2. Total Companies -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Companies</span>
                        <div class="metric-icon-wrapper bg-info-subtle text-info rounded-3 p-1" style="width: 32px; height: 32px;">
                            <i class="bi bi-buildings-fill"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-dark mb-0"><asp:Label ID="lblTotalCompanies" runat="server">0</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">Recruiters Visiting</small>
                </div>
            </div>
        </div>

        <!-- 3. Students Placed -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Placed</span>
                        <div class="metric-icon-wrapper bg-success-subtle text-success rounded-3 p-1" style="width: 32px; height: 32px;">
                            <i class="bi bi-check-circle-fill"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-success mb-0"><asp:Label ID="lblStudentsPlaced" runat="server">0</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">Offers Secured</small>
                </div>
            </div>
        </div>

        <!-- 4. Students Unplaced -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Seeking</span>
                        <div class="metric-icon-wrapper bg-warning-subtle text-warning rounded-3 p-1" style="width: 32px; height: 32px;">
                            <i class="bi bi-hourglass-split"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-dark mb-0"><asp:Label ID="lblStudentsNotPlaced" runat="server">0</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">In Drive Process</small>
                </div>
            </div>
        </div>

        <!-- 5. Total Offers -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Total Offers</span>
                        <div class="metric-icon-wrapper bg-danger-subtle text-danger rounded-3 p-1" style="width: 32px; height: 32px;">
                            <i class="bi bi-award-fill"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-dark mb-0"><asp:Label ID="lblTotalOffers" runat="server">0</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">Total Letters Issued</small>
                </div>
            </div>
        </div>

        <!-- 6. Placement Rate -->
        <div class="col-12 col-sm-6 col-xl-2">
            <div class="card metric-card border-0 shadow-sm rounded-3 h-100 overflow-hidden" style="border-left: 4px solid #8b5cf6 !important;">
                <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between mb-2">
                        <span class="text-uppercase text-secondary fw-semibold" style="font-size: 0.68rem;">Placement %</span>
                        <div class="metric-icon-wrapper text-purple rounded-3 p-1" style="background-color: #f3e8ff; color: #7e22ce; width: 32px; height: 32px;">
                            <i class="bi bi-graph-up-arrow"></i>
                        </div>
                    </div>
                    <h3 class="fw-bold text-primary mb-0"><asp:Label ID="lblPlacementPercentage" runat="server">0%</asp:Label></h3>
                    <small class="text-muted" style="font-size: 0.72rem;">Batch Conversion</small>
                </div>
            </div>
        </div>
    </div>

    <!-- Package Statistics Banner & Progress Bar -->
    <div class="card border-0 shadow-sm rounded-3 mb-4">
        <div class="card-body p-4">
            <div class="row g-4 align-items-center">
                <div class="col-12 col-md-4 text-center border-end-md">
                    <span class="text-muted small text-uppercase fw-semibold d-block">Highest Package Offered</span>
                    <h2 class="fw-bold text-primary mb-0"><asp:Label ID="lblHighestPackage" runat="server">₹0.00 LPA</asp:Label></h2>
                    <small class="text-muted">International / Top-tier tier 1 role</small>
                </div>
                <div class="col-12 col-md-4 text-center border-end-md">
                    <span class="text-muted small text-uppercase fw-semibold d-block">Average CTC Offered</span>
                    <h2 class="fw-bold text-success mb-0"><asp:Label ID="lblAveragePackage" runat="server">₹0.00 LPA</asp:Label></h2>
                    <small class="text-muted">Calculated across all accepted offers</small>
                </div>
                <div class="col-12 col-md-4">
                    <div class="d-flex justify-content-between align-items-center mb-1 small">
                        <span class="fw-semibold text-dark">Cohort Placement Progress</span>
                        <span class="fw-bold text-primary"><asp:Label ID="lblProgressText" runat="server">0%</asp:Label></span>
                    </div>
                    <div class="progress" style="height: 10px;">
                        <div id="divProgressBar" runat="server" class="progress-bar bg-success" role="progressbar" style="width: 0%;"></div>
                    </div>
                    <small class="text-muted mt-2 d-block text-center">Batch 2026-27 &bull; Engineering &amp; Technology</small>
                </div>
            </div>
        </div>
    </div>

    <!-- Section: Recent Placement Activities via ASP.NET Repeater Control -->
    <div class="card border-0 shadow-sm rounded-3 overflow-hidden">
        <div class="card-header bg-white py-3 px-4 d-flex align-items-center justify-content-between border-bottom">
            <div class="d-flex align-items-center gap-2">
                <div class="bg-warning-subtle text-warning rounded-2 p-2 d-flex align-items-center justify-content-center">
                    <i class="bi bi-repeat fs-5"></i>
                </div>
                <div>
                    <h5 class="mb-0 fw-bold text-dark">Recent Placement Activities</h5>
                    <small class="text-muted">Demonstrating <strong>ASP.NET Repeater Control</strong> with custom item templates</small>
                </div>
            </div>
            <a href="Placements.aspx" class="btn btn-outline-primary btn-sm rounded-pill px-3">
                View All Offers &rarr;
            </a>
        </div>

        <div class="card-body p-0">
            <!-- 4. ASP.NET REPEATER CONTROL IMPLEMENTATION -->
            <asp:Repeater ID="rptRecentPlacements" runat="server">
                <HeaderTemplate>
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light text-muted small text-uppercase">
                                <tr>
                                    <th class="ps-4">Student</th>
                                    <th>Department</th>
                                    <th>Recruiter Company</th>
                                    <th>Designation</th>
                                    <th>CTC Package</th>
                                    <th>Offer Date</th>
                                    <th class="text-end pe-4">Offer Status</th>
                                </tr>
                            </thead>
                            <tbody>
                </HeaderTemplate>

                <ItemTemplate>
                    <tr>
                        <td class="ps-4">
                            <div class="d-flex align-items-center gap-2">
                                <div class="bg-light border rounded-circle p-2 text-primary d-flex align-items-center justify-content-center" style="width: 36px; height: 36px;">
                                    <i class="bi bi-person-badge"></i>
                                </div>
                                <div>
                                    <span class="fw-semibold text-dark d-block"><%# Eval("StudentName") %></span>
                                    <small class="text-muted">ID: #<%# Eval("StudentID") %></small>
                                </div>
                            </div>
                        </td>
                        <td>
                            <span class="badge bg-light text-dark border"><%# Eval("Department") %></span>
                        </td>
                        <td>
                            <div class="fw-semibold text-dark"><i class="bi bi-building me-1 text-muted"></i> <%# Eval("CompanyName") %></div>
                            <small class="text-muted"><%# Eval("Location") %></small>
                        </td>
                        <td class="text-dark small"><%# Eval("Designation") %></td>
                        <td>
                            <span class="badge bg-success-subtle text-success border border-success-subtle font-monospace px-2 py-1">
                                ₹<%# string.Format("{0:0.00}", Eval("PackageOffered")) %> LPA
                            </span>
                        </td>
                        <td class="text-muted small">
                            <%# Convert.ToDateTime(Eval("OfferDate")).ToString("dd MMM yyyy") %>
                        </td>
                        <td class="text-end pe-4">
                            <span class="badge bg-success">
                                <i class="bi bi-check-lg me-1"></i> <%# Eval("Status") %>
                            </span>
                        </td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="table-light bg-opacity-25">
                        <td class="ps-4">
                            <div class="d-flex align-items-center gap-2">
                                <div class="bg-primary-subtle text-primary border rounded-circle p-2 d-flex align-items-center justify-content-center" style="width: 36px; height: 36px;">
                                    <i class="bi bi-person-badge"></i>
                                </div>
                                <div>
                                    <span class="fw-semibold text-dark d-block"><%# Eval("StudentName") %></span>
                                    <small class="text-muted">ID: #<%# Eval("StudentID") %></small>
                                </div>
                            </div>
                        </td>
                        <td>
                            <span class="badge bg-light text-dark border"><%# Eval("Department") %></span>
                        </td>
                        <td>
                            <div class="fw-semibold text-dark"><i class="bi bi-building me-1 text-muted"></i> <%# Eval("CompanyName") %></div>
                            <small class="text-muted"><%# Eval("Location") %></small>
                        </td>
                        <td class="text-dark small"><%# Eval("Designation") %></td>
                        <td>
                            <span class="badge bg-success-subtle text-success border border-success-subtle font-monospace px-2 py-1">
                                ₹<%# string.Format("{0:0.00}", Eval("PackageOffered")) %> LPA
                            </span>
                        </td>
                        <td class="text-muted small">
                            <%# Convert.ToDateTime(Eval("OfferDate")).ToString("dd MMM yyyy") %>
                        </td>
                        <td class="text-end pe-4">
                            <span class="badge bg-success">
                                <i class="bi bi-check-lg me-1"></i> <%# Eval("Status") %>
                            </span>
                        </td>
                    </tr>
                </AlternatingItemTemplate>

                <FooterTemplate>
                            </tbody>
                        </table>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoOffers" runat="server" Visible="false" CssClass="text-center py-5">
                <i class="bi bi-inbox text-muted fs-1 mb-2 d-block"></i>
                <p class="text-muted mb-0">No placement records found in database.</p>
            </asp:Panel>
        </div>
    </div>
</asp:Content>


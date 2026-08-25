<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentRegister.aspx.cs" Inherits="EventPortal.StudentRegister" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Event Registration Portal</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; }
        .form-container { width: 450px; padding: 20px; border: 1px solid #ccc; border-radius: 8px; }
        .form-group { margin-bottom: 15px; }
        .label { display: inline-block; width: 150px; font-weight: bold; }
        .input-field { width: 220px; padding: 5px; }
        .error { color: red; font-size: 13px; margin-left: 5px; }
        .btn-submit { background-color: #28a745; color: white; border: none; padding: 8px 16px; cursor: pointer; border-radius: 4px; }
        .btn-submit:hover { background-color: #218838; }
        .success { color: green; font-weight: bold; margin-top: 15px; display: block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Student Event Registration</h2>
            
            <asp:ValidationSummary ID="ValSummary" runat="server" ForeColor="Red" HeaderText="Please fix the following errors:" />

            <!-- Roll Number -->
            <div class="form-group">
                <asp:Label ID="lblRollNo" runat="server" Text="Roll Number:" CssClass="label" AssociatedControlID="txtRollNo" />
                <asp:TextBox ID="txtRollNo" runat="server" CssClass="input-field"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRollNo" runat="server" 
                    ControlToValidate="txtRollNo" ErrorMessage="Roll Number is required." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>

            <!-- Student Name -->
            <div class="form-group">
                <asp:Label ID="lblName" runat="server" Text="Student Name:" CssClass="label" AssociatedControlID="txtName" />
                <asp:TextBox ID="txtName" runat="server" CssClass="input-field"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                    ControlToValidate="txtName" ErrorMessage="Student Name is required." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>

            <!-- Branch -->
            <div class="form-group">
                <asp:Label ID="lblBranch" runat="server" Text="Branch:" CssClass="label" AssociatedControlID="ddlBranch" />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="input-field">
                    <asp:ListItem Value="">-- Select Branch --</asp:ListItem>
                    <asp:ListItem Value="CSE">Computer Science (CSE)</asp:ListItem>
                    <asp:ListItem Value="IT">Information Technology (IT)</asp:ListItem>
                    <asp:ListItem Value="ECE">Electronics (ECE)</asp:ListItem>
                    <asp:ListItem Value="ME">Mechanical (ME)</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" 
                    ControlToValidate="ddlBranch" InitialValue="" ErrorMessage="Please select a branch." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>

            <!-- Email -->
            <div class="form-group">
                <asp:Label ID="lblEmail" runat="server" Text="Email ID:" CssClass="label" AssociatedControlID="txtEmail" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                    ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ErrorMessage="Enter a valid email address." CssClass="error" Display="Dynamic">Invalid Email</asp:RegularExpressionValidator>
            </div>

            <!-- Mobile -->
            <div class="form-group">
                <asp:Label ID="lblMobile" runat="server" Text="Mobile No:" CssClass="label" AssociatedControlID="txtMobile" />
                <asp:TextBox ID="txtMobile" runat="server" CssClass="input-field" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" 
                    ControlToValidate="txtMobile" ErrorMessage="Mobile number is required." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" 
                    ControlToValidate="txtMobile" ValidationExpression="^[6-9]\d{9}$" 
                    ErrorMessage="Enter a valid 10-digit mobile number." CssClass="error" Display="Dynamic">Invalid Number</asp:RegularExpressionValidator>
            </div>

            <!-- Select Event -->
            <div class="form-group">
                <asp:Label ID="lblEvent" runat="server" Text="Select Event:" CssClass="label" AssociatedControlID="ddlEvent" />
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="input-field">
                    <asp:ListItem Value="">-- Select Event --</asp:ListItem>
                    <asp:ListItem Value="Coding">Hackathon / Coding Competition</asp:ListItem>
                    <asp:ListItem Value="WebDev">Web Design Contest</asp:ListItem>
                    <asp:ListItem Value="Gaming">LAN Gaming Fest</asp:ListItem>
                    <asp:ListItem Value="Quiz">Technical Quiz</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEvent" runat="server" 
                    ControlToValidate="ddlEvent" InitialValue="" ErrorMessage="Please select an event." 
                    CssClass="error" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>

            <!-- Submit Button -->
            <div class="form-group">
                <asp:Button ID="btnSubmit" runat="server" Text="Register Student" CssClass="btn-submit" OnClick="btnSubmit_Click" />
            </div>

            <!-- Message Label -->
            <asp:Label ID="lblMessage" runat="server" CssClass="success"></asp:Label>
        </div>
    </form>
</body>
</html>
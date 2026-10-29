# Experiment 7: Feedback Management Module

A complete, production-ready ASP.NET Core MVC application showcasing **MVC Layouts**, **Partial Views**, **Dual-Layer Validation (Data Annotations + Unobtrusive jQuery)**, and **Strongly-Typed HTML Helpers**.

---

## 🌟 Key Features & Architecture

### 1. MVC Layout (`Views/Shared/_Layout.cshtml`)
- **Master Template**: Provides consistent application shell, responsive navigation header, sticky brand bar, notifications container, and footer.
- **Section Rendering**: Demonstrates `@await RenderSectionAsync("Scripts", required: false)` for modular script injection (e.g. client-side validation libraries on form views).
- **Reusable Global Alerts**: Integrates `@await Html.PartialAsync("_AlertBanner")` to display dismissible `TempData` notifications across any page.

### 2. Partial Views Showcase
- **`Views/Feedback/_FeedbackForm.cshtml`**:
  - Encapsulates the entire feedback entry form.
  - Rendered inside `Create.cshtml` via `@await Html.PartialAsync("_FeedbackForm", Model)`.
- **`Views/Feedback/_FeedbackSummary.cshtml`**:
  - Displays high-level executive metrics (Total Submissions, Average Rating, Recommendation Rate %, Pending/Resolved counts, and 5-Star distribution bar chart).
- **`Views/Feedback/_FeedbackCard.cshtml`**:
  - Renders individual feedback cards in the responsive grid layout with dynamic star visualizers, sentiment indicators, and quick status controls.
- **`Views/Feedback/_FeedbackTable.cshtml`**:
  - Displays the administrative tabular listing for batch inspection, filtering, and direct ticket management.
- **`Views/Shared/_AlertBanner.cshtml`**:
  - Handles `TempData["SuccessMessage"]`, `TempData["ErrorMessage"]`, and `TempData["InfoMessage"]`.

### 3. Strongly-Typed HTML Helpers
The module makes extensive use of Razor HTML Helpers as requested:
- `@using (Html.BeginForm("Create", "Feedback", FormMethod.Post, new { id = "feedbackForm" }))`
- `@Html.AntiForgeryToken()`
- `@Html.ValidationSummary(false, "Summary Header", new { @class = "validation-summary-errors" })`
- `@Html.LabelFor(m => m.FullName)`
- `@Html.TextBoxFor(m => m.FullName, new { @class = "form-control" })`
- `@Html.ValidationMessageFor(m => m.FullName, "", new { @class = "validation-msg" })`
- `@Html.DropDownListFor(m => m.Category, Model.CategoryOptions, "-- Select Category --", new { @class = "form-select" })`
- `@Html.RadioButtonFor(m => m.Rating, 5, new { id = "star5", @class = "rating-radio" })`
- `@Html.TextAreaFor(m => m.Comments, 5, 50, new { @class = "form-control" })`
- `@Html.CheckBoxFor(m => m.WouldRecommend, new { @class = "form-check-input" })`
- `@Html.ActionLink("Back to Feedbacks", "Index", "Feedback", null, new { @class = "btn btn-secondary" })`

### 4. Dual-Layer Validation
- **Model-Level Data Annotations (`Models/Feedback.cs`)**:
  - `[Required]`: Ensures required fields are provided.
  - `[StringLength(60, MinimumLength = 3)]`: Enforces name bounds.
  - `[RegularExpression(@"^[a-zA-Z\s\.\'-]+$")]`: Disallows illegal name characters.
  - `[EmailAddress]`: Validates RFC email format.
  - `[RegularExpression(@"^[6-9]\d{9}$")]`: Enforces valid 10-digit Indian mobile number starting with 6-9.
  - `[Range(1, 5)]`: Enforces 1 to 5 star rating.
  - `[StringLength(1000, MinimumLength = 15)]`: Guarantees meaningful, non-empty feedback remarks.
- **Client-Side Validation**:
  - Rendered via `_ValidationScriptsPartial.cshtml` (`jquery.validate.min.js` and `jquery.validate.unobtrusive.min.js`).
  - Inputs receive `data-val-*` attributes generated automatically by the HTML Helpers.
  - Instantaneous feedback before network roundtrip, augmented with `:user-invalid` and `:user-valid` CSS indicators.
- **Server-Side Validation**:
  - Controller validates `ModelState.IsValid` before saving to repository.

---

## 🚀 Running the Project

### Prerequisites
- [.NET 10 / .NET SDK](https://dotnet.microsoft.com/)

### Start the Development Server
```bash
dotnet run --launch-profile http
```
The application will start listening on:
```
http://localhost:5190
```

### URLs & Navigation
- **Feedback Dashboard**: `http://localhost:5190` (or `http://localhost:5190/Feedback`)
- **Submit New Feedback**: `http://localhost:5190/Feedback/Create`
- **Feedback Details & Management**: `http://localhost:5190/Feedback/Details/{id}`

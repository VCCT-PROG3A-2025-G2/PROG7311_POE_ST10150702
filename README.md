<p align="center">
  <img src="/README_Files/circleLogo.png" alt="Agri-Energy Connect Logo" width="200"/>
</p>

<h1 align="center">Agri-Energy Connect</h1>
<p align="center"><strong>ST10150702</strong></p>
<br>

<h2 align="center">About this Project</h2>
<p align="center">
Agri-Energy Connect is a digital platform designed to bridge the gap between agricultural producers and sustainable energy solutions. 
This tool empowers farmers and energy experts to collaborate, track resource usage, and promote eco-friendly farming practices. 
With a focus on usability and data-driven insights, the system supports both environmental responsibility and agricultural productivity.
</p>
<br><br>

<p align="center">
| Jordan Muller | <br><br>
 <br>
</p>

<h2>Built With</h2>
<div>
  <a href="https://visualstudio.microsoft.com/vs/" target="_blank">
    <img src="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white" alt="Visual Studio 2022 Badge">
  </a>
  <br>
  <a href="https://github.com/" target="_blank">
    <img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white" alt="GitHub Badge">
  </a>
  <br>
  <a href="https://www.techtarget.com/whatis/definition/model-view-controller-MVC" target="_blank">
    <img src="https://img.shields.io/badge/MVC%20Architecture-005571?style=for-the-badge&logo=microsoft&logoColor=white" alt="MVC Badge">
  </a>
  <br>
  <a href="https://dotnet.microsoft.com/en-us/languages/csharp#:~:text=C%23%20is%20a%20modern%2C%20innovative,5%20programming%20languages%20on%20GitHub." target="_blank">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C# Badge">
  </a>
  <br>
  <a href="https://en.wikipedia.org/wiki/HTML" target="_blank">
    <img src="https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white" alt="HTML Badge">
  </a>
  <br>
  <a href="https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16" target="_blank">
    <img src="https://img.shields.io/badge/LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="LocalDB Badge">
  </a>
  <br><br>
  <a href="#top">(Back to Top)</a>
</div>

<br>
<h2>🚀 Getting Started</h2>

<p>This program was created using <strong>Visual Studio Community 2022</strong>.</p>

<h3>🛠️ Prerequisites</h3>

<ul>
  <li>
    <strong>Visual Studio</strong><br>
    Ensure you have <em>Visual Studio Community 2022</em> installed. You can download it by clicking 
    <a href="https://visualstudio.microsoft.com/vs/" target="_blank">here</a> and following the official installation guide.
  </li>
  <br>
  <li>
    <strong>Cloning the Repository</strong><br>
    Once Visual Studio is installed, copy the repository URL:<br>
    <code>https://github.com/JordanMuller039/PROG7311_POE_ST10150702.git</code>
    <br><br>
    Follow the steps shown in the images below:
    <br><br>
    <img src="README_Files/CloningRepo1.PNG" alt="Cloning Repo Step 1" style="width:25%;"><br>
    Navigate to <strong>"Clone a Repository"</strong> in Visual Studio.
    <br><br>
    <img src="README_Files/CloningRepo2.PNG" alt="Cloning Repo Step 2" style="width:45%;"><br>
    Paste the copied repo URL into the field shown above.
    <br><br>
    <img src="README_Files/CloningRepo3.PNG" alt="Cloning Repo Step 3" style="width:45%;"><br>
    Click <strong>"Clone"</strong> and allow Visual Studio to download the repository.
  </li>
</ul>

  <br>
  <h2>📦 Database Setup</h2>

<p>This project uses <strong>SQLite</strong> as its database engine, and the database file is included in the repository. No additional configuration or setup is required after cloning the project.</p>

<p>After cloning the repository:</p>
<ol>
  <li>Open the solution in <strong>Visual Studio 2022</strong>.</li>
  <li>Build and run the project. The application will connect to the pre-configured SQLite database.</li>
</ol>

<p><strong>✔️ Benefits:</strong></p>
<ul>
  <li>No need to install or configure SQL Server or LocalDB.</li>
  <li>Database schema and seed data are already included.</li>
</ul>

<p><strong>💡 Note:</strong> The database file is located at <code>wwwroot/data/AgriEnergyConnect.db</code>.</p>

<pre>
<code>
"ConnectionStrings": {
  "DefaultConnection": "Data Source=wwwroot/data/AgriEnergyConnect.db"
}
</code>
</pre>


<h2>📘 Usage</h2>

<p>This prototype doesn’t need to be fully functional, but the following pages are available within the project:</p>

<div align="center" style="margin-bottom: 1em;">
  <img src="https://img.shields.io/badge/Login-blue?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Register-green?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Farmer%20View-orange?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Employee%20View-purple?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Admin%20Dashboard-red?style=for-the-badge" />
</div>

<hr>

<h3>🔑 Login Page</h3>
<p>The user lands here first. From here, they can either log in using their credentials or click <strong>Register</strong> if they’re a first-time user.</p>
<img src="README_Files/Login.PNG" alt="Login Page" style="width:35%; display: block; margin: 1em auto;">

<h4>🔐 Login Role Handling</h4>
<p>The following code redirects the user to their respective dashboard based on their assigned role.</p>

<details>
  <summary><strong>View Login Code</strong></summary>
  <pre><code>
[HttpPost]
public async Task&lt;IActionResult&gt; Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return RedirectToAction("Dashboard", "Admin");
            else if (roles.Contains("Employee"))
                return RedirectToAction("EmployeeView", "Home");
            else if (roles.Contains("Farmer"))
                return RedirectToAction("FarmerView", "Home");
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    }
    return View(model);
}
  </code></pre>
</details>

<hr>

<h3>📝 Register Page</h3>
<p>Users must fill in several details to register. Upon successful registration, they’ll be redirected back to the login page to sign in.</p>
<img src="README_Files/Register.PNG" alt="Register Page" style="width:45%; display: block; margin: 1em auto;">

<h4>👤 Register Role & Profile Setup</h4>
<p>This code assigns the <code>Farmer</code> role by default and creates the corresponding profile.</p>

<details>
  <summary><strong>View Register Code</strong></summary>
  <pre><code>
[HttpPost]
public async Task&lt;IActionResult&gt; Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);
    var user = new ApplicationUser
    {
        UserName = model.Email,
        Email = model.Email,
        FirstName = model.FirstName,
        LastName = model.LastName
    };
    var result = await _userManager.CreateAsync(user, model.Password);
    if (result.Succeeded)
    {
        await _userManager.AddToRoleAsync(user, "Farmer");
        var farmer = new Farmer
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Region = model.Region,
            AcceptedPOPPIA = model.AcceptedPOPPIA,
            UserId = user.Id
        };
        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("FarmerView", "Home");
    }
    foreach (var error in result.Errors)
        ModelState.AddModelError(string.Empty, error.Description);
    return View(model);
}
  </code></pre>
</details>

<hr>

<h3>🌾 Farmer View</h3>
<p>This page allows Farmers to manage their products—view, add, or update their listings.</p>
<p>To access the Farmer View, use the pre-seeded login:</p>
<ul>
  <li>Email: <code>Jords@gmail.com</code></li>
  <li>Password: <code>Jords@123</code></li>
</ul>

<img src="README_Files/FarmerView.PNG" alt="Farmer View" style="width:45%; display: block; margin: 1em auto;">

<h4>📦 Add Product Logic (Farmer Only)</h4>
<p>The following code ensures the farmer exists and saves the product to the database.</p>

<details>
  <summary><strong>View AddProduct Code</strong></summary>
  <pre><code>
[HttpPost]
[Authorize(Roles = &quot;Farmer&quot;)]
[ValidateAntiForgeryToken]
public async Task&lt;IActionResult&gt; AddProduct(Product product)
{
    if (ModelState.IsValid)
    {
        try
        {
            var farmerExists = await _context.Farmers
                .AnyAsync(f =&gt; f.FarmerId == product.FarmerId);
            if (!farmerExists)
            {
                ModelState.AddModelError(&quot;&quot;, &quot;Invalid farmer specified&quot;);
                return View(&quot;FarmerView&quot;, product);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData[&quot;SuccessMessage&quot;] = $&quot;Product '{product.Name}' added successfully!&quot;;
            return RedirectToAction(&quot;FarmerView&quot;);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, &quot;Error adding product&quot;);
            ModelState.AddModelError(&quot;&quot;, &quot;Error saving product. Please try again.&quot;);
        }
    }
    var user = await _userManager.GetUserAsync(User);
    var farmer = await _context.Farmers.FirstOrDefaultAsync(f =&gt; f.UserId == user.Id);
    ViewBag.FarmerId = farmer?.FarmerId;
    ViewBag.FarmerFirstName = farmer?.FirstName;
    return View(&quot;FarmerView&quot;, product);
}
  </code></pre>
</details>

<hr>

<h3>👨‍🌾 Employee View</h3>
<p>Employees can view all farmers and products, add new farmers, and filter products by category or farmer.</p>
<p>To access the Employee View, use the pre-seeded login:</p>
<ul>
  <li>Email: <code>JohnM@gmail.com</code></li>
  <li>Password: <code>Emp@123</code></li>
</ul>
<img src="README_Files/EmployeeView.PNG" alt="Employee View" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h3>🛠️ Admin Dashboard</h3>

<p>The Admin Dashboard provides full control over users and product data within the system. A default admin account is pre-seeded for convenience:</p>

<ul>
  <li><strong>Email:</strong> <code>admin@farmcentral.com</code></li>
  <li><strong>Password:</strong> <code>Admin@1234!</code></li>
</ul>

<hr>

<h4>📊 Overview Tab</h4>
<p>After logging in, the Admin is greeted with an overview displaying system statistics and summaries:</p>
<img src="README_Files/AdminDashboard1.PNG" alt="Admin Overview" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h4>👨‍💼 Employees Tab</h4>
<p>This section allows the Admin to view, add, or delete Employees:</p>
<img src="README_Files/AdminDashboard2.PNG" alt="Employees View" style="width:100%; display: block; margin: 1em auto;">

<p>Clicking <strong>"Add Employee"</strong> opens a form to input new employee details:</p>
<img src="README_Files/AdminDashboard3.PNG" alt="Add Employee" style="width:50%; display: block; margin: 1em auto;">

<hr>

<h4>🚜 Farmers Tab</h4>
<p>Admins can manage all Farmer accounts here — create, view, or remove them as needed:</p>
<img src="README_Files/AdminDashboard4.PNG" alt="Farmers View" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h4>🛒 Products Tab</h4>
<p>In this tab, Admins can manage all product records. They can view all products in the system or remove any as needed:</p>
<img src="README_Files/AdminDashboard5.PNG" alt="Products View" style="width:100%; display: block; margin: 1em auto;">

<br>

<h2>🗃️ Database Entries</h2>

<p>
The database is pre-created and seeded with temporary data to ensure the prototype runs smoothly out of the box.
Below are previews of the key seeded tables:
</p>

<hr>

<h4>👨‍🌾 Farmers Table</h4>
<p>Contains all registered farmers, seeded with example users to demonstrate functionality.</p>
<img src="README_Files/SELECT_Farmers.PNG" alt="Farmers Table" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h4>🛒 Products Table</h4>
<p>Displays all products currently stored in the system, each associated with a farmer.</p>
<img src="README_Files/SELECT_Products.PNG" alt="Products Table" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h4>👔 Employees Table</h4>
<p>Includes all employee records managed via the Admin Dashboard.</p>
<img src="README_Files/SELECT_Employees.PNG" alt="Employees Table" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h4>🔐 Roles Table</h4>
<p>Defines user roles used for role-based access control within the system (e.g., Admin, Employee, Farmer).</p>
<img src="README_Files/SELECT_ROLES.PNG" alt="Roles Table" style="width:100%; display: block; margin: 1em auto;">

<br>
<h2>🗺️ Roadmap</h2>

<h3>✅ Completed Features</h3>
<ul>
  <li><strong>Create Working Relational Database</strong> - SQLite database with proper relationships</li>
  <li><strong>Develop Two Distinct Roles (Farmer & Employee)</strong> - Role-based authentication system</li>
  <li><strong>Farmers can Add Products & View Own Products</strong> - Complete CRUD functionality</li>
  <li><strong>Employees can View all Products & Add Farmers</strong> - Administrative features</li>
  <li><strong>Secure Login Functionality with Authentication</strong> - ASP.NET Core Identity</li>
  <li><strong>User-Friendly UX/UI</strong> - Responsive design with intuitive navigation</li>
  <li><strong>Data Validation & Error Checking</strong> - Form validation and error handling</li>
  <li><strong>Populate Database with Sample Data</strong> - Pre-seeded users and products</li>
</ul>

<h3>🔜 Planned Features</h3>
<ul>
  <li><strong>Develop Marketplace</strong> - Trading platform for farmers</li>
  <li><strong>Create Forums Page</strong> - Community discussion board</li>
  <li><strong>Ensure Mobile-Friendly UX/UI</strong> - Enhanced mobile responsiveness</li>
  <li><strong>Create Education Page</strong> - Resources for sustainable farming</li>
</ul>

<h3>📊 Progress</h3>

<div style="margin: 1em 0;">
  <strong>Prototype:</strong><br>
  <img src="https://img.shields.io/badge/Progress-100%25-brightgreen?style=for-the-badge" alt="Prototype Progress">
</div>

<div style="margin: 1em 0;">
  <strong>Full App:</strong><br>
  <img src="https://img.shields.io/badge/Progress-60%25-orange?style=for-the-badge" alt="Full App Progress">
</div>

<a href="#top">(Back to Top)</a>
<br>

<h2>Acknowlegements</h2>
<h3>Reference List</h3>
freeCodeCamp (2024) Model-view architecture: A comprehensive guide. Available at: https://www.freecodecamp.org/news/model-view-architecture/ (Accessed: 7-14 May 2024).
<br><br>
freeCodeCamp (2024) How to write a good README file. Available at: https://www.freecodecamp.org/news/how-to-write-a-good-readme-file/ (Accessed: 7-14 May 2024).
<br><br>
Microsoft (2024) ASP.NET overview. Available at: https://learn.microsoft.com/en-us/aspnet/overview (Accessed: 7-14 May 2024).
<br><br>
Microsoft (2024) Entity Framework documentation. Available at: https://learn.microsoft.com/en-us/aspnet/entity-framework (Accessed: 7-14 May 2024).
<br><br>
Microsoft (2024) Introduction to ASP.NET Identity. Available at: https://learn.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity (Accessed: 7-14 May 2024).
<br><br>
SQLite Tutorial (2024) SQLite programming tutorials. Available at: https://www.sqlitetutorial.net/ (Accessed: 7-14 May 2024).
<br><br>
Contentsquare (2024) UX design examples and best practices. Available at: https://contentsquare.com/guides/ux-design/examples/ (Accessed: 7-14 May 2024).
<br><br>
<h3>AI Usage</h3>
In the course of this project, AI tools were utilized as a supplementary resource to aid in understanding and exploring certain coding techniques. These tools were integrated into our workflow to enhance our understanding but were not employed to perform tasks autonomously, ensuring that all development work was carried out by the project team.
<br>
OpenAI: OpenAI, 2025. ChatGPT. Available at: https://openai.com [Accessed 7-14 May 2025].
<br>
Deepseek AI: Deepseek AI, 2025. Deepseek AI. Available at: https://deepseek.ai [Accessed 7-14 May 2025].

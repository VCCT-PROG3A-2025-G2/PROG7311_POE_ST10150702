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

<p>After cloning the repository, follow these steps to initialize the database using Entity Framework Core and LocalDB:</p>

<ol>
  <li>Open the solution in <strong>Visual Studio 2022</strong>.</li>
  <li>Go to <code>Tools</code> → <code>NuGet Package Manager</code> → <code>Package Manager Console</code>.</li>
  <li>In the Package Manager Console, run the following commands:</li>
</ol>

<pre>
<code>
Add-Migration InitialCreate
Update-Database
</code>
</pre>

<p>These commands will:</p>
<ul>
  <li>Generate the initial database schema from your models.</li>
  <li>Create and seed the local database (if configured).</li>
</ul>

<p><strong>💡 Tip:</strong> Ensure your <code>appsettings.json</code> file includes a valid LocalDB connection string, such as:</p>

<pre>
<code>
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDbName;Trusted_Connection=True;"
}
</code>
</pre>
</p>

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
<img src="README_Files/EmployeeView.PNG" alt="Employee View" style="width:100%; display: block; margin: 1em auto;">

<hr>

<h3>🛠️ Admin Dashboard</h3>
<p>To access the Admin Dashboard, use the pre-seeded login:</p>
<ul>
  <li>Email: <code>admin@farmcentral.com</code></li>
  <li>Password: <code>Admin@1234!</code></li>
</ul>
<p>This account is auto-created in <code>Program.cs</code> on startup.</p>
<img src="README_Files/AdminLogin.PNG" alt="Admin Login" style="width:45%; display: block; margin: 1em auto;">





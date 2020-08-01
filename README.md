# GameStore

#Database Tables (Entities)
![Tables](https://i.ibb.co/1rVd5Rd/image.png)

#Pages:
Home Page
![Home Page](https://i.ibb.co/grsvtvP/image.png)
Categories
![Categories](https://i.ibb.co/nQQ7WpC/image.png)
Games
![Games](https://i.ibb.co/qmzchKw/image.png)
Game Details
![Game Details](https://i.ibb.co/SdzSMmq/image.png)
Companies
![Companies](https://i.ibb.co/y45txCR/image.png)
Search
![Search](https://i.ibb.co/XbG52H0/image.png)
Updates and Patches
![Updates](https://i.ibb.co/bdDw08g/image.png)

#Database and Hosting Information On the Cloud:
![Database](https://i.ibb.co/KDGkD8f/image.png)
![Web App](https://i.ibb.co/NZBJRNg/image.png)

#Carousel
![Home Page](https://i.ibb.co/grsvtvP/image.png)

#Bonus
Search: Search has date time picker and it performs seraches on datetime (from release date/to release date) on database. which also filters the results.
Also Game model implements a virtual int datatype that is actually calculates the time difference of the release date and current date (game age)
![Search](https://i.ibb.co/XbG52H0/image.png)

#Authorize minimum 1 action result in a controller
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,CEO,CompanyDescription,Address,Phone")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
 ![Access Denied](https://i.ibb.co/h7RY5MP/image.png)
  
  
  #Hide minimum 2 Links based on role
  <div class="list-group list-group-flush">
                <a asp-controller="Categories" asp-action="Index" data-controller="categories" con class="list-group-item list-group-item-action bg-light">Categories</a>
                <a asp-controller="Games" asp-action="Index" data-controller="games" class="list-group-item list-group-item-action bg-light">Games</a>
                <a asp-controller="Companies" asp-action="Index" data-controller="companies" class="list-group-item list-group-item-action bg-light">Companies</a>
                <a asp-controller="Updates" asp-action="Index" data-controller="updates" class="list-group-item list-group-item-action bg-light">Updates/Patches/Skins</a>
               @if (User.IsInRole("Admin"))
               {
                <a asp-controller="Roles" asp-action="Index" data-controller="roles" class="list-group-item list-group-item-action bg-light">Roles</a>
                <a asp-controller="UserRoles" asp-action="Index" data-controller="userroles" class="list-group-item list-group-item-action bg-light">User Roles</a>
               }
            </div>
            
#Authorize by multiple roles on a controller based on roles

     [Authorize(Roles="Admin,Support")]
        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
  

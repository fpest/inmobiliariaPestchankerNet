using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace inmobiliariaPestchanker.Controllers

{

   

  
public class UsuariosController : Controller
    {
   private readonly IConfiguration configuration;
   private readonly IWebHostEnvironment environment;

    

   public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
   private RepositorioUsuario repo = new RepositorioUsuario();
   private RepositorioRol repoRol = new RepositorioRol();  
    [Authorize]
   public ActionResult Index()
        {
        try{
          var lista = repo.ObtenerTodos();
           ViewBag.Rol = repoRol.ObtenerTodos();
            
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
            return View(lista);
          }catch(Exception e){
            throw;
          }
        }

       [Authorize]
       public ActionResult Details(int id)
        {
            try{
            var entidad = repo.ObtenerPorId(id);
            ViewBag.Rol = repoRol.ObtenerPorId(entidad.IdRol);
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
            return View(entidad);

          }catch(Exception e){
            throw;
          }


           
        }

      
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
           {

                      try{

           ViewBag.Rol = repoRol.ObtenerTodos();
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
            return View();
          }catch(Exception e){

            throw;
          }
        }

        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]

        public ActionResult Create(Usuario usuario)
              {
         //  if (!ModelState.IsValid)
         //      return View();
          try
            {

            var RolActual = repoRol.ObtenerPorId(usuario.IdRol).Descripcion;

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usuario.Clave = hashed;
               
               
               // usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repo.Alta(usuario);


                if (usuario.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                }else{

                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar.png";
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    
                }

                repo.Modificacion(usuario);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

             ViewBag.Rol = repoRol.ObtenerTodos();
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
                return View();
            }
        }

    [Authorize]
    public ActionResult Edit(int id)
   
       


    {

            
        var entidad = repo.ObtenerPorEmail(User.Identity.Name);
     
     if (User.IsInRole("Administrador")){
        entidad = repo.ObtenerPorId(id);
        ViewBag.Admin = "Administrador";
     }else {
        
            ViewBag.Admin = "Empleado";

     }


                  try{
      //    var entidad = repo.ObtenerPorId(id);
           ViewBag.Rol = repoRol.ObtenerTodos();
         var usua = Request.Headers["referer"].FirstOrDefault();
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
            return View(entidad);
          }catch(Exception e){

            throw;
          }
           
            }
        

        // POST: Inquilinos/Edit/5
        [HttpPost]
//        [ValidateAntiForgeryToken]
  
      [Authorize]

       public ActionResult Edit(int id, Usuario usuario)
        {
            Usuario i = null;
            try
            {

        var Miemail = User.Identity.Name;        

        if (User.IsInRole("Administrador")){
        ViewBag.Admin = "Administrador";
        }else {
            ViewBag.Admin = "Empleado";

     }
         
          i = repo.ObtenerPorId(id);
                i.Nombre = usuario.Nombre;
                i.Apellido = usuario.Apellido;
                i.Dni = usuario.Dni;
                
                
                i.Email = usuario.Email;
                i.IdRol = usuario.IdRol;
       //AvatarFile
             if (usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                        i.Avatar = usuario.Avatar;
                    }
  
                repo.Modificacion(i);
   
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try{

              var entidad = repo.ObtenerPorId(id);
              ViewBag.Rol = repoRol.ObtenerPorId(entidad.IdRol);
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
            return View(entidad);


            }
            catch(Exception e){
            throw;
            }
            
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
               repo.Baja(id);
               return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Mensaje"] = "No se pudo Borrar el Registro";
                return RedirectToAction(nameof(Index));
                //  return View();
            }
        }
 
        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult LoginModal()
        {
            return PartialView("_LoginModal", new LoginView());
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string)? "/Home" : TempData["returnUrl"].ToString();                
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repo.ObtenerPorEmail(login.Usuario);
                    
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    var RolNombre = repoRol.ObtenerPorId(e.IdRol).Descripcion;
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, RolNombre),
                        
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
 
    


      // POST: Inquilinos/Edit/5
        [HttpPost]
//        [ValidateAntiForgeryToken]
  
      [Authorize]

       public ActionResult EditClave(int id, String Clave)
        {
            try
            {

          string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                Clave = hashed;
              
      
      
                repo.ModificacionClave(id,Clave);
                
                return RedirectToAction("Index", "Usuarios"); 
            }
            catch
            {
                return View();
            }
        }
  
    }








}
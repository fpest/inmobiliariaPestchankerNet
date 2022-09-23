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

   

  
public class ClaveController : Controller
    {
   private readonly IConfiguration configuration;
   private readonly IWebHostEnvironment environment;

    

   public ClaveController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
   private RepositorioClave repo = new RepositorioClave();
   private RepositorioRol repoRol = new RepositorioRol();  
   
   private RepositorioUsuario repoUsuario = new RepositorioUsuario();



 [Authorize]
   public ActionResult Index()
        {
        try{

          var lista = repoUsuario.ObtenerTodos();
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
    public ActionResult Edit(int id)
   
    {



                  try{
          var entidad = repo.ObtenerPorId(id);
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
            return View(entidad);
          }catch(Exception e){

            throw;
          }
           
    }
    
    
    
       [HttpPost]

      [Authorize]
       public ActionResult Edit(int id, Clave clave)
        {
            
            var claveAnterior= clave.ClaveAnterior;
            var claveNueva = clave.ClaveNueva;
            
            
            try
            {

          string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: claveAnterior,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                claveAnterior = hashed;
              
        string hashedNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: claveNueva,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                claveNueva = hashedNueva;

        Usuario u = repoUsuario.ObtenerPorId(id);

        if (u.Clave == claveAnterior){

                repo.ModificacionClave(id,claveNueva);
                
                return RedirectToAction("Index", "Usuarios"); 
           

        }
        else{ return View();}
      
             }
            catch
            {
                return View();
            }
        }

    
    
    }
    
    }
     /*    

        // POST: Inquilinos/Edit/5
        [HttpPost]
//        [ValidateAntiForgeryToken]
  
      [Authorize]

     //  public ActionResult Edit(int id, Clave clave)
   
    
    
     public ActionResult Edit()
   { 
   {    
      
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
         
          i = repoUsuario.ObtenerPorId(id);
                
               
                if (usuario.Clave!=null){
                       //i.Clave = usuario.Clave;
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                i.Clave = hashed;
                }else{
                        usuario.Clave=i.Clave;
                }
                
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
        
       */
      /*
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

 */

 








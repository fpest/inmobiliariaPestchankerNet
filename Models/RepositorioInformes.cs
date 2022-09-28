using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioInformes : RepositorioBase
	{
		public RepositorioInformes() : base()
		{

		}






				public String obtenerDiaActual(){
				DateTime hoy = DateTime.Today;
				string diaActual = hoy.ToString("yyyy-MM-dd");


				return diaActual; 
				}


//		ObtenerListaFiltradaContrato

public string arreglarFecha(String fecha){

fecha = fecha.Substring(0,10);
String Dia = fecha.Substring(0,2);
String Mes = fecha.Substring(3,2);
String Year = fecha.Substring(6,4);
fecha = Year + "-" + Mes + "-" + Dia;



return fecha;
}



public IList<InformeContratos> ObtenerListaFiltradaContrato(FiltrarContrato filtrarContrato)
		{

			var sqlWhereFi = "";
			var sqlWhereFf = "";
			var sqlWhereIn = "";
			var sqlWhere = "";
			
			if (filtrarContrato.FechaDesde.HasValue || filtrarContrato.FechaHasta.HasValue){
				if(!filtrarContrato.FechaDesde.HasValue && filtrarContrato.FechaHasta.HasValue){
					filtrarContrato.FechaDesde=filtrarContrato.FechaHasta;
				}
				if(filtrarContrato.FechaDesde.HasValue && !filtrarContrato.FechaHasta.HasValue){
					filtrarContrato.FechaHasta=filtrarContrato.FechaDesde;
				}

				  	String fechaDesde = filtrarContrato.FechaDesde.ToString();
        			String fechaHasta = filtrarContrato.FechaHasta.ToString();
					
					


				sqlWhere = "Where ('"+ arreglarFecha(fechaDesde) + "' >= cont.FechaInicio  && '" + arreglarFecha(fechaDesde) + "' <= cont.FechaFin) || ('" + arreglarFecha(fechaHasta) +"' >= cont.FechaInicio && '" + arreglarFecha(fechaHasta) + "' <= cont.FechaFin) || ('" + arreglarFecha(fechaDesde) +"' <= cont.FechaInicio && '" + arreglarFecha(fechaHasta) + "' >= cont.FechaFin)";
					
					//cont.FechaInicio < '" + filtrarContrato.FechaDesde + "' And " + "cont.FechaFin <  '" + filtrarContrato.FechaHasta + "'"; 
			
				if (filtrarContrato.IdInquilino == -1) {sqlWhere = sqlWhere + "And IdInquilino > -1 ";}
				else{sqlWhere = sqlWhere + "And IdInquilino = " + filtrarContrato.IdInquilino;}				
			
			
			
			}else{
				if (filtrarContrato.IdInquilino == -1) {sqlWhere = "Where IdInquilino > -1 ";}
				else{sqlWhere = "Where IdInquilino = " + filtrarContrato.IdInquilino;}		}		
				






			IList<InformeContratos> res = new List<InformeContratos>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			
			
				string sql = @"SELECT 
				cont.Id As IdContrato,
				inq.Id As IdInquilino,
				inm.Direccion, 
				cont.FechaInicio, 
				cont.FechaFin, 
				cont.Precio,
				pro.Id As IdPropietario,
				inq.Nombre As NombreInquilino,
				inq.Apellido As ApellidoInquilino,
				pro.Nombre As NombrePropietario,
				pro.Apellido As ApellidoPropietario
				FROM contrato cont
				inner join inmueble inm on inm.Id = cont.IdInmueble
                inner join inquilino inq on inq.Id = cont.IdInquilino
				inner join propietario pro on inm.IdPropietario=pro.Id " + sqlWhere;
		
	
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					
					{
						
						InformeContratos i = new InformeContratos
						
						{

							IdContrato = reader.GetInt32(0),
							IdInquilino = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							FechaInicio = reader.GetDateTime(3),
							FechaFin = reader.GetDateTime(4),
							Precio = reader.GetDecimal(5),
							IdPropietario = reader.GetInt32(6),
							NombreInquilino = reader.GetString(7),
							ApellidoInquilino = reader.GetString(8),
							NombrePropietario = reader.GetString(9),
							ApellidoPropietario = reader.GetString(10)

						
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


		public IList<InformeInmuebles> ObtenerTodosLista()
		{
			IList<InformeInmuebles> res = new List<InformeInmuebles>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			
			
			string sql = @"SELECT 
				pro.Nombre,
				pro.Apellido,
				Direccion, 
				CantidadAmbientes, 
				PrecioInmueble, 
				Disponible,
				inm.Id As Id,
				pro.Id As IdPropietario,
				cont.FechaInicio,
				cont.FechaFin
				FROM inmueble inm 
				inner join propietario pro on inm.IdPropietario=pro.Id 
				inner join tipoinmueble tinm on inm.IdTipoInmueble = tinm.Id
				inner join contrato cont on inm.Id = cont.IdInmueble";
		
	
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					
					{
						
						InformeInmuebles i = new InformeInmuebles
						
						{

							Duenio = new Propietario
                            {
                                Nombre = reader.GetString(0),
                                Apellido = reader.GetString(1),
								Id = reader.GetInt32(7)
							},
							Direccion = reader.GetString(2),
							CantidadAmbientes = reader.GetInt32(3),
							PrecioInmueble = reader.GetDecimal(4),
							Disponible = reader.GetBoolean(5),
							Id = reader.GetInt32(6),
								FechaInicio = reader.GetDateTime(8),
							FechaFin = reader.GetDateTime(9)


						
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

public IList<InformeInmuebles> ObtenerListaFiltrada(FiltrarInmuebles filtrarInmuebles)
		{

			var sqlWhereD = "";
			var sqlWhereP = "";
			var sqlWhereO = "";
			var sqlWhere = "";
			if (filtrarInmuebles.Disponibles != "Todos"){

				if(filtrarInmuebles.Disponibles == "Disponibles"){
					sqlWhereD="Disponible=true";
				}else{
					sqlWhereD="Disponible=false";
				}
			}else{sqlWhereD = "inm.Id > -1";} //solo para que elija todos

			if (filtrarInmuebles.IdPropietario != -1){

					sqlWhereP="pro.Id = " + filtrarInmuebles.IdPropietario;
				
			}else{sqlWhereP = "pro.Id > -1";}//solo para que elija todos

			if (filtrarInmuebles.Ocupados != "Todos"){
										
				if(filtrarInmuebles.Ocupados == "Ocupado"){

					String hoySql = obtenerDiaActual();

					sqlWhereO= "FechaInicio <= '" + hoySql +"' and FechaFin >= '" + hoySql + "'";

				}else{
					String hoySql = obtenerDiaActual();

					sqlWhereO="(FechaInicio >= '" + hoySql +"' or FechaFin <= '" + hoySql + "')"; 
				}


			}else{sqlWhereO = "FechaInicio >= '1900-01-01'";}//solo para que elija todos
	

			sqlWhere = "Where " + sqlWhereD + " and " + sqlWhereP + " and " + sqlWhereO;


			IList<InformeInmuebles> res = new List<InformeInmuebles>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			
			
				string sql = @"SELECT 
				pro.Nombre,
				pro.Apellido,
				Direccion, 
				CantidadAmbientes, 
				PrecioInmueble, 
				Disponible,
				inm.Id As Id,
				pro.Id As IdPropietario,
				cont.FechaInicio,
				cont.FechaFin
				FROM inmueble inm 
				inner join propietario pro on inm.IdPropietario=pro.Id 
				inner join tipoinmueble tinm on inm.IdTipoInmueble = tinm.Id
				inner join contrato cont on inm.Id = cont.IdInmueble " + sqlWhere;
		
	
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					
					{
						
						InformeInmuebles i = new InformeInmuebles
						
						{

							Duenio = new Propietario
                            {
                                Nombre = reader.GetString(0),
                                Apellido = reader.GetString(1),
								Id = reader.GetInt32(7)
							},
							Direccion = reader.GetString(2),
							CantidadAmbientes = reader.GetInt32(3),
							PrecioInmueble = reader.GetDecimal(4),
							Disponible = reader.GetBoolean(5),
							Id = reader.GetInt32(6),
							FechaInicio = reader.GetDateTime(8),
							FechaFin = reader.GetDateTime(9)


						
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}



	}
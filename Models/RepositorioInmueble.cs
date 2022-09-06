using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioInmueble : RepositorioBase
	{
		public RepositorioInmueble() : base()
		{

		}

		public int Alta(Inmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Inmueble 
				(IdPropietario, Direccion, CoordenadaN, CoordenadaE, 
				IdTipoInmueble, TipoUso, CantidadAmbientes, 
				PrecioInmueble, Disponible) 
				VALUES (@IdPropietario, @Direccion, @CoordenadaN, 
				@CoordenadaE, @IdTipoInmueble, @TipoUso, @CantidadAmbientes, 
				@PrecioInmueble, @Disponible);" +
					"SELECT LAST_INSERT_ID();";
					//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
					command.Parameters.AddWithValue("@Direccion", i.Direccion);
					command.Parameters.AddWithValue("@CoordenadaN", i.CoordenadaN);
					command.Parameters.AddWithValue("@CoordenadaE", i.CoordenadaE);
					command.Parameters.AddWithValue("@IdTipoInmueble", i.IdTipoInmueble);
                    command.Parameters.AddWithValue("@TipoUso", i.TipoUso);
                    command.Parameters.AddWithValue("@CantidadAmbientes", i.CantidadAmbientes);
                    command.Parameters.AddWithValue("@PrecioInmueble", i.PrecioInmueble);
                    command.Parameters.AddWithValue("@Disponible", i.Disponible);
                    
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
				}
			}
			return res;
		}

		
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmueble WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

	
				string sql = @"UPDATE Inmueble SET IdPropietario=@IdPropietario, 
				Direccion=@Direccion, CoordenadaN=@CoordenadaN, 
				CoordenadaE=@CoordenadaE, IdTipoInmueble=@IdTipoInmueble, 
				TipoUso=@TipoUso, CantidadAmbientes=@CantidadAmbientes, 
				PrecioInmueble=@PrecioInmueble, Disponible=@Disponible 
				WHERE Id = @id";
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
					command.Parameters.AddWithValue("@Direccion", i.Direccion);
					command.Parameters.AddWithValue("@CoordenadaN", i.CoordenadaN);
					command.Parameters.AddWithValue("@CoordenadaE", i.CoordenadaE);
					command.Parameters.AddWithValue("@IdTipoInmueble", i.IdTipoInmueble);
                    command.Parameters.AddWithValue("@TipoUso", i.TipoUso);
                    command.Parameters.AddWithValue("@CantidadAmbientes", i.CantidadAmbientes);
                    command.Parameters.AddWithValue("@PrecioInmueble", i.PrecioInmueble);
                    command.Parameters.AddWithValue("@Disponible", i.Disponible);
					command.Parameters.AddWithValue("@id", i.Id);
					
                    connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		
		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, IdPropietario, Direccion, CoordenadaN, CoordenadaE, IdTipoInmueble, TipoUso, CantidadAmbientes, PrecioInmueble, Disponible" +
                    $" FROM Inmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							IdPropietario = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							CoordenadaN = reader.GetDecimal(3),
                            CoordenadaE = reader.GetDecimal(4),
							IdTipoInmueble = reader.GetInt32(5),
							TipoUso = reader.GetString(6),
                            CantidadAmbientes = reader.GetInt32(7),
                            PrecioInmueble = reader.GetDecimal(8),
                            Disponible = reader.GetBoolean(9)
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, IdPropietario, Direccion, CoordenadaN, CoordenadaE, IdTipoInmueble, TipoUso, CantidadAmbientes, PrecioInmueble, Disponible" +
					$" FROM Inmueble LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}";
					
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							IdPropietario = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							CoordenadaN = reader.GetDecimal(3),
                            CoordenadaE = reader.GetDecimal(4),
							IdTipoInmueble = reader.GetInt32(5),
							TipoUso = reader.GetString(6),
                            CantidadAmbientes = reader.GetInt32(7),
                            PrecioInmueble = reader.GetDecimal(8),
                            Disponible = reader.GetBoolean(9)
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


		virtual public Inmueble ObtenerPorId(int id)
		{

			
			Inmueble i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			string sql = @"SELECT inm.Id, IdPropietario, Direccion, CoordenadaN, 
			CoordenadaE, IdTipoInmueble, TipoUso, CantidadAmbientes, 
			PrecioInmueble, Disponible, pro.Nombre, pro.Apellido, 
			tinm.Descripcion As TipoInmueble
			From inmueble inm
			inner join propietario pro on inm.IdPropietario=pro.Id
			inner join tipoinmueble tinm on inm.IdTipoInmueble = tinm.Id
			WHERE inm.Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
						Id = reader.GetInt32(0),
							IdPropietario = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							CoordenadaN = reader.GetDecimal(3),
                            CoordenadaE = reader.GetDecimal(4),
							IdTipoInmueble = reader.GetInt32(5),
							TipoUso = reader.GetString(6),
                            CantidadAmbientes = reader.GetInt32(7),
                            PrecioInmueble = reader.GetDecimal(8),
                            Disponible = reader.GetBoolean(9),
							Duenio = new Propietario{
								Id = reader.GetInt32(1),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),

							}

						};
					}
					connection.Close();
				}
			}
			return i;
        }

		public IList<InmuebleLista> ObtenerTodosLista()
		{
			IList<InmuebleLista> res = new List<InmuebleLista>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			
			
				string sql = @"SELECT CONCAT(pro.Apellido,', ',pro.Nombre) AS Propietario, 
				Direccion, CantidadAmbientes, PrecioInmueble, 
				Disponible, inm.Id As Id FROM inmueble inm 
				inner join propietario pro on inm.IdPropietario=pro.Id 
				inner join tipoinmueble tinm on inm.IdTipoInmueble = tinm.Id";
		
	
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					
					{
						
						InmuebleLista i = new InmuebleLista
						
						{
							Propietario = reader.GetString(0),
							Direccion = reader.GetString(1),
							CantidadAmbientes = reader.GetInt32(2),
							PrecioInmueble = reader.GetDecimal(3),
							Disponible = reader.GetBoolean(4),
							Id = reader.GetInt32(5)


						
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

/*

{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, IdPropietario, Direccion, CoordenadaN, CoordenadaE, IdTipoInmueble, TipoUso, CantidadAmbientes, PrecioInmueble, Disponible" +
                    $" FROM Inmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							IdPropietario = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							CoordenadaN = reader.GetDecimal(3),
                            CoordenadaE = reader.GetDecimal(4),
							IdTipoInmueble = reader.GetInt32(5),
							TipoUso = reader.GetString(6),
                            CantidadAmbientes = reader.GetInt32(7),
                            PrecioInmueble = reader.GetDecimal(8),
                            Disponible = reader.GetBoolean(9)
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;




        public Propietario ObtenerPorEmail(string email)
        {
            Propietario p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE Email=@email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();
            Propietario p = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

	}
*/


}



using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioRol : RepositorioBase
	{
		public RepositorioRol() : base()
		{

		}

/*
		public int Alta(TipoInmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO tipoinmueble (Descripcion) 
				VALUES (@Descripcion);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					
					command.Parameters.AddWithValue("@Descripcion", i.Descripcion);
				    
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
				string sql = $"DELETE FROM tipoinmueble WHERE Id = @id";
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
		public int Modificacion(TipoInmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE tipoinmueble SET Descripcion=@Descripcion WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Descripcion", i.Descripcion);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

*/


			virtual public Rol ObtenerPorId(int id)
		{

			
			Rol i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			string sql = @"SELECT Id, Descripcion
			From rol 
			WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Rol
						{
							Id = reader.GetInt32(0),
							Descripcion = reader.GetString(1),
							
						};
					}
					connection.Close();
				}
			}
			return i;
        }


		public IList<Rol> ObtenerTodos()
		{
			IList<Rol> res = new List<Rol>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Descripcion  FROM rol";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Rol i = new Rol
						{
							Id = reader.GetInt32(0),
							Descripcion = reader.GetString(1),
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


/*
		virtual public TipoInmueble ObtenerPorId(int id)
		{
			
			TipoInmueble i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
			string sql = @"SELECT Id, Descripcion  FROM tipoinmueble 
			WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new TipoInmueble
						{
							Id = reader.GetInt32(0),
							Descripcion = reader.GetString(1),

							
						};
					}
					connection.Close();
				}
			}
			return i;
        }

/*
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
			PrecioInmueble, Disponible, pro.Nombre, pro.Apellido
			From inmueble inm
			inner join propietario pro on inm.IdPropietario=pro.Id

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



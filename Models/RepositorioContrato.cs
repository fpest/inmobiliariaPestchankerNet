using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioContrato : RepositorioBase
	{
		public RepositorioContrato() : base()
		{

		}

		public int Alta(Contrato i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Contrato (IdInmueble, IdInquilino, 
				FechaInicio, FechaFin, Precio)
				 VALUES (@IdInmueble, @IdInquilino, @FechaInicio,
				 @FechaFin, @Precio);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", i.IdInmueble);
					command.Parameters.AddWithValue("@IdInquilino", i.IdInquilino);
					command.Parameters.AddWithValue("@FechaInicio", i.FechaInicio);
					command.Parameters.AddWithValue("@FechaFin", i.FechaFin);
					command.Parameters.AddWithValue("@Precio", i.Precio);
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
				string sql = $"DELETE FROM Contrato WHERE Id = @id";
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

		
		public int Modificacion(Contrato i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = @"UPDATE contrato SET 
				IdInmueble=@IdInmueble, 
				IdInquilino=@IdInquilino, 
				FechaInicio=@FechaInicio, 
				FechaFin=@FechaFin,
				Precio=@Precio
				WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", i.IdInmueble);
					command.Parameters.AddWithValue("@IdInquilino", i.IdInquilino);
					command.Parameters.AddWithValue("@FechaInicio", i.FechaInicio);
					command.Parameters.AddWithValue("@FechaFin", i.FechaFin);
					command.Parameters.AddWithValue("@Precio", i.Precio);
					command.Parameters.AddWithValue("@id", i.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		
		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT cont.Id, IdInmueble, IdInquilino, FechaInicio,
				 FechaFin, Precio, inq.Apellido, inq.Nombre, inm.Direccion 
				 FROM contrato cont
				 join inmueble inm on cont.IdInmueble = inm.Id
				 join inquilino inq on cont.IdInquilino = inq.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato i = new Contrato
						{
							Id = reader.GetInt32(0),
							IdInmueble = reader.GetInt32(1),
							IdInquilino = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),						
							FechaFin = reader.GetDateTime(4),
							Precio = reader.GetDecimal(5),
							Inquilino = new Inquilino{
								Id = reader.GetInt32(2),
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(6),
							},
							Inmueble = new Inmueble{
								Id = reader.GetInt32(1),
								Direccion = reader.GetString(8),
														
							},

							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


	virtual public Contrato ObtenerPorId(int id)
		{
			Contrato i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT cont.Id, IdInmueble, IdInquilino, FechaInicio,
				 FechaFin, Precio, inq.Apellido, inq.Nombre, inm.Direccion 
				 FROM contrato cont
				 join inmueble inm on cont.IdInmueble = inm.Id
				 join inquilino inq on cont.IdInquilino = inq.Id 
				 WHERE cont.Id=@id";
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Contrato
						{
							Id = reader.GetInt32(0),
							IdInmueble = reader.GetInt32(1),
							IdInquilino = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),						
							FechaFin = reader.GetDateTime(4),
							Precio = reader.GetDecimal(5),
							Inquilino = new Inquilino{
								Id = reader.GetInt32(2),
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(6),
							},
							Inmueble = new Inmueble{
								Id = reader.GetInt32(1),
								Direccion = reader.GetString(8),
														
							},

							
						};
					};
					connection.Close();
				}
			}
			return i;
			}

			
			
        





/*
		public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilino
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader["Telefono"].ToString(),
							Email = reader.GetString(5),
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilino" +
					$" WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							
						};
					}
					connection.Close();
				}
			}
			return i;
        }
*/


/*
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



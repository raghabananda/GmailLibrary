using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace GmailSystem
{
    public class Global
    {
        string connectionString ="Data Source=(local);Initial Catalog=EmailSystem;Integrated Security=True";
        string DeleteCommand = "DELETE FROM [Profile] WHERE [Id] = @Id";
        string InsertCommand = "INSERT INTO [Profile] ([First Name], [Las Name], [Sex], [Email Id], [Password], [Dob], [Photo]) VALUES (@First_Name, @Las_Name, @Sex, @Email_Id, @Password, @Dob, @Photo);";
        string SelectCommand = "SELECT [Id], [First Name] AS First_Name, [Las Name] AS Las_Name, [Sex], [Email Id] AS Email_Id, [Password], [Dob], [Photo] FROM [Profile]";
        string UpdateCommand = "UPDATE [Profile] SET [First Name] = @First_Name, [Las Name] = @Las_Name, [Sex] = @Sex, [Email Id] = @Email_Id, [Password] = @Password, [Dob] = @Dob, [Photo] = @Photo WHERE [Id] = @Id";
        string Icmd="INSERT INTO [Mail] ([Sender_Profile_id], [Receiver_Profile_id], [Subject], [Text]) VALUES (@Sender_Profile_id, @Receiver_Profile_id, @Subject, @Text)";
      public  string InsertData(string First_Name, string Last_Name, string Sex, string Email_Id, string Password, string Dob)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                int i = CheckEmailIdExistsOrNot(Email_Id);
                if (i > 0)
                {
                    return "This Email Id already exists.";
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = InsertCommand + "SELECT  SCOPE_IDENTITY() AS I";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@First_Name",First_Name);
                    cmd.Parameters.AddWithValue("@Las_Name", Last_Name);
                    cmd.Parameters.AddWithValue("@Sex", Sex);
                    cmd.Parameters.AddWithValue("@Email_Id", Email_Id);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Dob", Dob);
                    if (Sex.Equals("Male"))
                        cmd.Parameters.AddWithValue("@Photo", "Photo/Male.png");
                    else
                        cmd.Parameters.AddWithValue("@Photo", "Photo/Female.png");

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0].Rows[0]["I"].ToString();
                }
            }
        }
      public string SendMail(string SenderProfile_id, string Email_Adress, string Subject, string Text)
      {
         using(SqlConnection con = new SqlConnection(connectionString))
          {
              int i = CheckEmailIdExistsOrNot(Email_Adress);
              if(i==0)
              {
                  return "Email Id Doesn't exist.";
              }
              else
              {
              SqlCommand cmd = new SqlCommand();
              cmd.CommandText ="INSERT INTO [Mail] ([Sender_Profile_id], [Receiver_Profile_id], [Subject], [Text]) VALUES (@Sender_Profile_id, @Receiver_Profile_id, @Subject, @Text)";
              cmd.Connection=con;

              cmd.Parameters.AddWithValue("@Sender_Profile_id", Convert.ToInt32(SenderProfile_id));
              cmd.Parameters.AddWithValue("@Receiver_Profile_id", Convert.ToInt32(ReturnIdByEmail(Email_Adress)));
              cmd.Parameters.AddWithValue("@Subject", Subject);
              cmd.Parameters.AddWithValue("@Text", Text);

              con.Open();
              cmd.ExecuteNonQuery();
              return "Message has been sent successfully";
              }
          }
      }
      public void ChangePhoto(string ImageUrl,int Id)
      {
          using (SqlConnection con = new SqlConnection(connectionString))
          {
              SqlCommand cmd = new SqlCommand();
              cmd.CommandText ="UPDATE [Profile] SET [Photo] = @Photo WHERE [Id] = @Id";
              cmd.Connection = con;
              cmd.Parameters.AddWithValue("@Photo", ImageUrl);
              cmd.Parameters.AddWithValue("@Id",Id);

              con.Open();
              cmd.ExecuteNonQuery();
          }
      }
      public void ChangePassword(int Id, string NewPassword)
      {
          using (SqlConnection con = new SqlConnection(connectionString))
          {
              SqlCommand cmd = new SqlCommand();
              cmd.CommandText = "UPDATE [Profile] SET [Password] = @Password WHERE [Id] = @Id";
              cmd.Connection = con;
              cmd.Parameters.AddWithValue("@Password", NewPassword);
              cmd.Parameters.AddWithValue("@Id", Id);

              con.Open();
              cmd.ExecuteNonQuery();
          }
      }
      public DataSet ReturnDataset(string SqlQuery,int id)
      {
          DataSet ds = new DataSet();
          using (SqlConnection con = new SqlConnection(connectionString))
          {
              SqlCommand cmd = new SqlCommand(SqlQuery,con);
              cmd.Parameters.AddWithValue("@Receiver_Profile_id", id);
              con.Open();
              SqlDataAdapter da = new SqlDataAdapter(cmd);
              
              da.Fill(ds);
          }
          return ds;
      }
        public string SetImageUrl(int Id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [Photo] FROM [Profile] WHERE [Id] = @Id";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Id", Id);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return (ds.Tables[0].Rows[0]["Photo"]).ToString();
            }
        }
        public int CheckEmailIdExistsOrNot(string Email)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT COUNT( [Email Id]) FROM [Profile] WHERE [Email Id] = @Email_Id ";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Email_Id", Email);               

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public string ReturnIdByEmail(string Email)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [Id] FROM [Profile] WHERE [Email Id] = @Email_Id ";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Email_Id", Email);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds,"Table_0");
            }
            return ds.Tables["Table_0"].Rows[0]["Id"].ToString();
        }
        public string ReturnPasswordByEmail(string Email)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [Password] FROM [Profile] WHERE [Email Id] = @Email_Id ";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Email_Id", Email);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Table_0");
            }
            return ds.Tables["Table_0"].Rows[0]["Password"].ToString();
        }
        public string ReturnFirstNameByEmail(string Email)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [First Name] AS First_Name FROM [Profile] WHERE [Email Id] = @Email_Id ";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Email_Id", Email);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Table_0");
            }
            return ds.Tables["Table_0"].Rows[0]["First_Name"].ToString();
        }
    }
}

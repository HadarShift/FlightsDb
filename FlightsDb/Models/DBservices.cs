using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using FlightsDb.Models;
/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;
    int counter = 0;
    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }




    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes a car to the movie table CLASSEX
    //--------------------------------------------------------------------------------------------------

    public int delete(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("carsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteCommand(id);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }



    //--------------------------------------------------------------------------------------------------
    // This method inserts a car to the movie table CLASSEX
    //--------------------------------------------------------------------------------------------------
    /// <summary>
    /// הכנסת יעדים לפני העלאת התוכנית
    /// </summary>
    public int insert(List<Destinations> destinations)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("destinationsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        try
        {
            String cStr = "";
            int numEffected = 0;
            foreach (var item in destinations)
            {
                if (counter == 681)
                {
                    
                }
                cStr = BuildInsertCommand(item);      // helper method to build the insert string
                cmd = CreateCommand(cStr, con);             // create the command
                numEffected += cmd.ExecuteNonQuery(); // execute the command

            }


            return numEffected;
            }

        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();

            }
        }

    }

    /// <summary>
    /// שמירת טיסות ששבחרתי
    /// </summary>
    public int SaveFlight(Flights flight)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("destinationsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        try
        {
            String cStr = "";
            int numEffected = 0;
            cStr = BuildSaveFlightQry(flight);      // שומר את הטיסה שבחרתי
            cmd = CreateCommand(cStr, con);             // create the command
            numEffected += cmd.ExecuteNonQuery(); // execute the command
            //שמירת הקונקשיינים של הטיסה שבחרתי
            string cityFrom="",cityTo="";
             for (int i = 0; i < flight.Routes.Count; i++)
            {

                if (i == 0)
                    cityFrom = flight.cityFrom;
                else
                    cityFrom = flight.Routes[i-1].ToString();
                if(i != flight.Routes.Count - 1)
                    cityTo = flight.Routes[i].ToString();
                else  
                    cityTo = flight.cityTo;

                cStr = BuildConncectionRoutes(flight.FlightId,cityFrom,cityTo);
                cmd = CreateCommand(cStr, con);             // create the command
                numEffected += cmd.ExecuteNonQuery();
            }

            return numEffected;
        }

        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();

            }
        }
    }


    //--------------------------------------------------------------------
    // Build the Insert command method String for MOVIE CLASSEX
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Destinations destination)
    {

        String command;
        
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "";
        destination.City = destination.City.Replace("'", "''");

        sb.AppendFormat("Values('{0}',{1},{2},'{3}')", destination.City, destination.LenLat, destination.LenLon, destination.Code);
            prefix = "INSERT INTO Airport_2020 " + "(city, Lenlot,Leclong,code) ";
            command = prefix + sb.ToString();
         counter++;

        return command;
    }

    /// <summary>
    /// בניית שאילתה עבור הכנסת טיסות שאני רוצה
    /// </summary>
    private String BuildSaveFlightQry(Flights flight)
    {

        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "";
        sb.AppendFormat("Values('{0}','{1}','{2}','{3}','{4}')",flight.FlightId,flight.dateFrom,flight.dateUntil,flight.cityFrom,flight.cityTo);
        prefix = "INSERT INTO MyFlights " + "(FlightNum,DateFrom,DateTo,CityFrom,CityTo) ";
        command = prefix + sb.ToString();

        return command;
    }
    /// <summary>
    /// קונקשיינים
    /// </summary>
    private String BuildConncectionRoutes(string FlightId, string cityFrom,string cityTo)
    {

        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "";
        sb.AppendFormat("Values('{0}','{1}','{2}')", FlightId, cityFrom, cityTo);
        prefix = "INSERT INTO RoutesConnection " + "(FlightNum,CityFrom,CityTo) ";
        command = prefix + sb.ToString();

        return command;
    }

    /// <summary>
    /// מייבא נתונים מהדטה בייס על יעדים
    /// </summary>
    internal List<Destinations> importData()
    {
        List<Destinations> destinationsList = new List<Destinations>();
        Destinations objDest = new Destinations();
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("destinationsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        try
        {
            String cStr = $@"SELECT *
                        FROM Airport_2020"; ;
            cmd = CreateCommand(cStr, con);             // create the command
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    objDest.City = reader["city"].ToString();
                    objDest.LenLat = double.Parse(reader["Lenlot"].ToString());
                    objDest.LenLon = double.Parse(reader["Leclong"].ToString());
                    objDest.Code = reader["code"].ToString();
                    destinationsList.Add(objDest);
                    objDest = new Destinations();
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        return destinationsList;
   
    }
    //--------------------------------------------------------------------
    // Build the Insert command method String for MOVIE CLASSEX
    //--------------------------------------------------------------------
    private String BuildDeleteCommand(int id)
    {
        String command;

        //StringBuilder sb = new StringBuilder();
        //// use a string builder to create the dynamic string
        //sb.AppendFormat("Values('{0}', '{1}')", movie.Name, movie.Actor);
        //String prefix = "INSERT INTO movie_2020 " + "(Name, Actor) ";
        //command = prefix + sb.ToString();

        command = "delete from movie_2020 where movieid = " + id.ToString();

        return command;
    }



    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    /// <summary>
    /// מחזיר טיסות שבחרתי
    /// </summary>
    internal List<Flights> ReturnFlightsChosen()
    {
        List<Flights> FlightsList = new List<Flights>();
        Flights objFlight = new Flights();
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("destinationsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        try
        {
            String cStr = $@"SELECT *
                             FROM [dbo].[MyFlights]";
            cmd = CreateCommand(cStr, con);             // create the command
            SqlDataReader reader2;
            reader2= cmd.ExecuteReader();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    objFlight.FlightId = reader2["FlightNum"].ToString();
                    objFlight.dateFrom = reader2["DateFrom"].ToString();
                    objFlight.dateUntil = reader2["DateTo"].ToString();
                    objFlight.cityFrom = reader2["CityFrom"].ToString();
                    objFlight.cityTo= reader2["CityTo"].ToString();
                    objFlight.InitialRoutesList();//מאתחל רשימת קונקשיינים ואחר כך יכניס אותם
                    FlightsList.Add(objFlight);
                    objFlight = new Flights();
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader2.Close();

         

        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        return FlightsList;
    }
    /// <summary>
    /// מוסיף לרשימת טיסות שבחרתי את הקונקשיינים
    /// </summary>
    /// <returns></returns>
    internal List<Flights> AddConnenctionRoutes(List<Flights> FligthsList)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("destinationsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        //לאסוף את הקונקשיינים של אותה טיסה
        string cStr = "";
        for (int i = 0; i < FligthsList.Count; i++)
        {
            cStr = $@"SELECT R.CityTo
                        FROM [dbo].[MyFlights] as F inner join [dbo].[RoutesConnection] as R on F.FlightNum=R.FlightNum
                        WHERE F.FlightNum='{FligthsList[i].FlightId.ToString()}'";
            cmd = CreateCommand(cStr, con);// create the command
            SqlDataReader readerForRoutes = cmd.ExecuteReader();
            if (readerForRoutes.HasRows)
            {
                while (readerForRoutes.Read())
                {
                    FligthsList[i].Routes.Add(readerForRoutes["CityTo"].ToString());
                }
                readerForRoutes.Close();
            }
        }
        return FligthsList;
    }

}

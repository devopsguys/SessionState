using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SessionStatePerformanceTestMVC.Common
{
    public abstract class SessionStateManager
    {
        // Start Timer
        DateTime _startTime;

        public void Start()
        {
            if (!CheckSessionStateIsEnabled()) return;
            HttpContext.Current.Response.Write(string.Format("<h1>Session State Mode = {0}</h1>", HttpContext.Current.Session.Mode));
            _startTime = DateTime.Now;
        }

        public void Finish()
        {
            // Finish timer and display timetaken
            DateTime endTime = DateTime.Now;
            TimeSpan timeTaken = endTime - _startTime;
            HttpContext.Current.Response.Write(string.Format("<h3>Time Taken (ms) = {0}</h3>", timeTaken.Milliseconds));

            // Get Size of Session
            HttpContext.Current.Response.Write(string.Format("<h3>Size (kb) = {0}</h3>", GetSizeOfObject(HttpContext.Current.Session.Contents)));
        }

        public virtual void AddDataToSession(int sizeOfTestData)
        {
            if (!CheckSessionStateIsEnabled()) return;

            HttpContext.Current.Response.Write("<h3>Adding items to session...</h3>");
            HttpContext.Current.Response.Write(string.Format("Items now in session = {0}", HttpContext.Current.Session.Contents.Count));
        }

        public virtual  void LoadDataFromSession()
        {
            if (!CheckSessionStateIsEnabled()) return;

            HttpContext.Current.Response.Write("<h3>Loading items from session...</h3>");
           
            foreach (var crntSession in HttpContext.Current.Session)
            {
                var key = crntSession;
                var value = HttpContext.Current.Session[crntSession.ToString()];
            }

            HttpContext.Current.Response.Write(string.Format("Items loaded from session = {0}", HttpContext.Current.Session.Contents.Count));
        }

        public virtual  void RemoveDataFromSession()
        {
            if (!CheckSessionStateIsEnabled()) return;

            HttpContext.Current.Response.Write("<h3>Removing items from session...</h3>");
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Response.Write(string.Format("Items now in session = {0}", HttpContext.Current.Session.Contents.Count));
        }

        protected bool CheckSessionStateIsEnabled()
        {
            if (HttpContext.Current.Session == null)
            {
                HttpContext.Current.Response.Write("<h1>Session state is disabled.</h1>");
                return false;
            }
            if (HttpContext.Current.Session.Mode == System.Web.SessionState.SessionStateMode.Off)
            {
                HttpContext.Current.Response.Write("<h1>Session state is disabled.</h1>");
                return false;
            }
            return true;
        }

        private long GetSizeOfObject(object o)
        {
            long size = 0;
            return size;
        }
    }

    public class SimpleSessionStateManager : SessionStateManager
    {
        public override void AddDataToSession(int sizeOfTestData)
        {
            if (!base.CheckSessionStateIsEnabled()) return;

            Random rnd = new Random();
            for (int i = 0; i < sizeOfTestData; i++)
            {
                // String
                string sessionDataAsString = GetRandomString();
                HttpContext.Current.Session.Add(string.Format("basic_string_{0}", i), sessionDataAsString);

                // Integer                
                int sessionDataAsInt = rnd.Next(i, sizeOfTestData);
                HttpContext.Current.Session.Add(string.Format("basic_integer_{0}", i), sessionDataAsInt);

                // Double
                double stringDataAsDouble = rnd.NextDouble();
                HttpContext.Current.Session.Add(string.Format("basic_double_{0}", i), stringDataAsDouble);

                // DateTime
                DateTime stringDataAsDateTime = GetRandomDate(rnd, DateTime.Now.AddYears(-500), DateTime.Now.AddYears(500));
                HttpContext.Current.Session.Add(string.Format("basic_datetime_{0}", i), stringDataAsDateTime);

                // TimeSpan
                TimeSpan stringDataAsTimeSpan = TimeSpan.FromDays(i);
                HttpContext.Current.Session.Add(string.Format("basic_timespan_{0}", i), stringDataAsTimeSpan);

                // Guid
                Guid stringDataAsGuid = Guid.NewGuid();if (!CheckSessionStateIsEnabled()) return;
                HttpContext.Current.Session.Add(string.Format("basic_guid_{0}", i), stringDataAsGuid);
            }

            base.AddDataToSession(sizeOfTestData);
        }

        private DateTime GetRandomDate(Random rnd, DateTime from, DateTime to)
        {
            TimeSpan range = new TimeSpan(to.Ticks - from.Ticks);
            return from + new TimeSpan((long)(range.Ticks * rnd.NextDouble()));
        }

        private string GetRandomString()
        {
            string random = System.IO.Path.GetRandomFileName().Replace(".", string.Empty);
            return random;
        }
    }

    public class ComplexSessionStateManager : SessionStateManager
    {
        public override void AddDataToSession(int sizeOfTestData)
        {

            // Generic List
            for (int i = 0; i < sizeOfTestData; i++)
            {
                List<object> list = new List<object>();
                list.Add(new object());
                list.Add(new object());
                list.Add(new object());
                HttpContext.Current.Session.Add(string.Format("GenericList_{0}", i), list);
            }

            // Array List
            for (int i = 0; i < sizeOfTestData; i++)
			{
                ArrayList list = new ArrayList();
                list.Add(new object());
                list.Add(new object());
                list.Add(new object());
                HttpContext.Current.Session.Add(string.Format("ArrayList_{0}", i), list);
			}

            // Custom Object
            for (int i = 0; i < sizeOfTestData; i++)
            {
                Album album = new Album()
                {
                    ArtistId = i,
                    Artist = new Artist() { ArtistId = i, Name = "Demo" },
                    AlbumId = i,
                    GenreId = i,
                    Genre = new Genre() { GenreId = i, Name = "Rock", Description = "Demo" },
                    Price = 9.99m,
                    Title = string.Format("Rock Masterclass volume {0}", i),
                    AlbumArtUrl = "http://demo"
                };

                HttpContext.Current.Session.Add(string.Format("Album_{0}", i), album);
            }
        
            // DataSet
            for (int i = 0; i < sizeOfTestData; i++)
            {
                DataSet dataSet = new DataSet(string.Format("Music_{0}", i));
                DataTable table1 = new DataTable(String.Format("table_{0}", i));
                table1.Columns.Add("id");
                table1.Columns.Add("name");
                table1.Rows.Add(1, "Data 1");
                table1.Rows.Add(2, "Data 2");
                dataSet.Tables.Add(table1);
                HttpContext.Current.Session.Add(string.Format("DataSet_{0}", i), dataSet);
            }
            
            base.AddDataToSession(sizeOfTestData);
        }
    }
}
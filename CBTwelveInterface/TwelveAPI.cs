using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CBTwelveInterface
{
    public class TwelveAPI
    {
        private static HttpClientHandler clientHandler;
        private static HttpClient client;

        public static void Initialize()
        {
            clientHandler = new HttpClientHandler {

                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()

            };

            client = new HttpClient(clientHandler);
        }

        //Beware ids without parantheses
        public static async Task<IList<TwelveAssignmentPersonal>> PostGetAssignments(string assignmentID, string studentpersonalID)
        {
            const string quote = @"""";
            const string backslash = @"\";
            
            string json = "{"+quote+"$where" + quote + ":" + quote + "it.AssignmentID==Guid("+backslash+quote+assignmentID+backslash + quote +")&& it.StudentPersonalID==Guid(" + backslash + quote +studentpersonalID + backslash + quote +")" + quote + "," + quote + "$loadOptions" + quote + ":[" + quote + "Assignment" + quote + "," + quote + "Status" + quote + "," + quote + "SectionInfo" + quote + "]}";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_GET_ASSIGNMENTS),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                  //  { "HARID", "893837566" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" },
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json")

            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();
            IList<TwelveAssignmentPersonal> tap = JsonConvert.DeserializeObject<IList<TwelveAssignmentPersonal>>(resultContent);

            return tap;
        }

        public static async Task<IList<TwelveCalendarItem>> PostStudentCalendarItems(int skip, int take, string enrollmentID)
        {
            const string quote = @"""";
            const string backslash = @"\";
            string json = "{" + quote + "$orderby" + quote + ":" + quote + "it.StartDate desc" + quote + "," + quote + "$where"+quote+":"+quote+"it.Type=="+backslash+quote+"Assignment"+backslash+quote+quote+"," + quote +"$skip" + quote + ":" + skip + "," + quote + "$take" + quote + ":" + take + "," + quote + "$includeTotalCount" + quote + ":true," + quote + "EnrollmentID" + quote + ":" + quote + enrollmentID + quote + "," + quote + "FilterMode" + quote + ":" + quote + "Yearly" + quote + "," + quote + "Increment" + quote + ":1," + quote + "Keyword" + quote + ":" + quote + quote + "}";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_STUDENT_CALENDAR_ITEMS),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                  //  { "HARID", "893837566" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" },
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json")

            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();

            IList<TwelveCalendarItem> tap = JsonConvert.DeserializeObject<IList<TwelveCalendarItem>>(resultContent);

            return tap;

        }

        public static async Task PostGetStudentPictures()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            const string quote = @"""";
            const string backslash = @"\";
            
            dic.Add("$where", "it.StudentPersonalID==Guid("+ quote+"{ edfb7ada-ad1f-5e88-22a2-bdd2522c66fd}"+ quote+") &&it.SchoolYearID==2022");

            //string json = System.Text.Json.JsonSerializer.Serialize(dic);

            string json = "{"+quote+"$where"+quote+":"+quote+"it.StudentPersonalID==Guid("+backslash+quote+"{edfb7ada-ad1f-5e88-22a2-bdd2522c66fd}"+backslash+quote+")&&it.SchoolYearID==2022"+quote+"}";


            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_GET_STUDENT_PICTURES),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" },
                    { "Accept-Encoding", "gzip, deflate, br" }
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json")

            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();

            Console.WriteLine("sugosssma");

        }

        public static async Task GetDefaultWebpage()
        {
            await client.GetAsync(KTwelveAdresses.GET_DEFAULT_WEBPAGE);
        }

        public static async Task GetSPTSWebpage()
        {
            await client.GetAsync(KTwelveAdresses.GET_SPTS_WEBPAGE);
        }

        public static async Task<TwelveStudentSchoolEnrollment> GetSchoolEnrollment()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.GET_STUDENT_SCHOOL_ENROLLMENT),
                Method = HttpMethod.Get,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HARID", "-1426793458" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" }
                }
            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();

            try
            {
                List<TwelveStudentSchoolEnrollment> user = JsonConvert.DeserializeObject<List<TwelveStudentSchoolEnrollment>>(resultContent);
                return user[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Source);
            }
            return new TwelveStudentSchoolEnrollment();
        }

        public static async Task<TwelveUser> GetUser()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.GET_USER),
                Method = HttpMethod.Get,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HARID", "-666965058" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" }
                }
            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();


            TwelveUser user = JsonConvert.DeserializeObject<TwelveUser>(resultContent);
            return user;
        }

        public static async Task<TwelveStudentPersonal> GetStudentPersonal()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.GET_STUDENT_PERSONAL),
                Method = HttpMethod.Get,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HARID", "1645553066" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" }
                }
            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();


            List<TwelveStudentPersonal> menu = JsonConvert.DeserializeObject<List<TwelveStudentPersonal>>(resultContent);
            return menu[0];
        }

        public static async Task PostGetProcessDomainResult()
        {

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_GET_PROCESS_DOMAIN_RESULT),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HARID", "3938" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" },
                },
                Content = new StringContent("{}", Encoding.UTF8, "application/json")

            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();


        }

        public static async Task<TwelveMenu> PostMenu()
        {

            var request = new HttpRequestMessage { 
                RequestUri = new Uri(KTwelveAdresses.GET_PORTALS), 
                Method = HttpMethod.Get,
                Headers =
                {
                    { "AppID", "3d7f07f7-6ec1-4aa0-b7a4-02c3df408759" },
                    { "HARID", "-1115909177" },
                    { "HAppID", "186987042" },
                    { "Cache-Control", "no-cache" } 
                }
            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();


            List <TwelveMenu> menu = JsonConvert.DeserializeObject<List<TwelveMenu>>(resultContent);
            return menu[0];
        }

        public static async Task<TwelveUserInfo> PostUserInfo()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_USER_INFO),
                Method = HttpMethod.Post,
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            var result = await client.SendAsync(request);

            string resultContent = await result.Content.ReadAsStringAsync();

            TwelveUserInfo info = JsonConvert.DeserializeObject<TwelveUserInfo>(resultContent);

            return info;
        }

        public static async Task<bool> PostLoginRequest(TwelveCredentials credentials)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("userName", credentials.Username);
            dic.Add("password", credentials.Password);
            dic.Add("createPersistentCookie", false);

            string json = System.Text.Json.JsonSerializer.Serialize(dic);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(KTwelveAdresses.POST_LOGIN),
                Method = HttpMethod.Post,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var result = await client.SendAsync(request);
            string resultContent = await result.Content.ReadAsStringAsync();

            return resultContent.Contains("true");

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Models;
using tw.com.essentialoil.AdminLTE.ViewModels;
using prjShoppingForum.Models.Entity;
using static tw.com.essentialoil.AdminLTE.ViewModels.CAdminLTETime;
using Newtonsoft.Json;

namespace tw.com.essentialoil.AdminLTE.Models
{
    public class CAdminLTE
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        //當前一周時間
        public CWeek queryWeek()
        {
            CWeek week = new CAdminLTETime.CWeek();
            DateTime nowDay = DateTime.UtcNow.AddHours(8);

            //將目前一周對應的時間回傳
            week.Monday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Monday));
            week.Tuesday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Tuesday));
            week.Wednesday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Wednesday));
            week.Thursday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Thursday));
            week.Friday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Friday));
            week.Saturday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Saturday));
            week.Sunday = nowDay.AddDays(-(nowDay.DayOfWeek - DayOfWeek.Sunday));

            return week;
        }

        //算出當天的營業額
        public int DayofRevenue(DateTime day)
        {
            int Revenue = 0;
            var Day = from od in db.tOrderDetails
                      where od.tOrder.fOrderDate.Day == day.Day
                      select od;

            foreach (var item in Day)
            {
                Revenue += (int)item.fOrderQuantity*(int)item.fUnitPrice;
            }

            return Revenue;
        }

        public int[] revenue()
        {
            //算出個個天數的營業額
            int Mon = DayofRevenue(queryWeek().Monday);
            int Tue = DayofRevenue(queryWeek().Tuesday);
            int Wed = DayofRevenue(queryWeek().Wednesday);
            int Thu = DayofRevenue(queryWeek().Thursday);
            int Fri = DayofRevenue(queryWeek().Friday);
            int Sat = DayofRevenue(queryWeek().Saturday);
            int Sun = DayofRevenue(queryWeek().Sunday);

            //將各個天數的營業額加到List
            List<int> Week = new List<int> { Mon, Tue, Wed, Thu, Fri, Sat, Sun };

            int[] data = new int[] { Mon, Tue, Wed, Thu, Fri, Sat, Sun } ;

            return data;
        }
    }
}
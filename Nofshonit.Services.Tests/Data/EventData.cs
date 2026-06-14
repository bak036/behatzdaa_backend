using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.Tests.Data
{
    public static class EventData
    {
        /// <summary>
        /// key: categoryId, value : event count
        /// </summary>
        public static Dictionary<long, int> GetEventsByCategoryIdQtyTest_Data()
        {
            Dictionary<long, int> model = new Dictionary<long, int>();
            model.Add(42733, 1);          

            return model;
        }
    }
}

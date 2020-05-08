using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class PictureLog : ILog
    {

        public PictureLog(Bitmap bitmap, LogType logType, Action urgency) {
            string adress = "Logs/Images/" + Guid.NewGuid().ToString() + ".png";
            bitmap.Save(adress);

            Type = logType;
            Message = "PictureLog was saved to adress: " + adress;
            Created = DateTime.Now;
            Urgency = urgency;
        }
    }
}

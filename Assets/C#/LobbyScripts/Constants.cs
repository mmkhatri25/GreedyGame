using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyScripts
{
    public static class Constants
    {
      public static  string BaseURL = "https://jeetogame.in/jeeto_game/WebServices/";
      public static  string LoginURL = BaseURL+"loginPassword";
      public static  string GuestURL = BaseURL + "Guestlogin";
      public static  string UpdateProfileURL = BaseURL + "updateProfile";
      public static  string ProfileURL = BaseURL + "profile";
      public static  string RankURL = BaseURL + "myRank";
      public static  string SafeLockerURL = BaseURL + "safeLocker";
      public static string AddDiamondURL = BaseURL + "AddDiamond";
      public static string DiamondOfferURL = BaseURL + "DiamondOfferDatails";
      public static string UPLOAD_PROFILE_URL = "OnUploadImage";

    }

    public class BackEndData<T> where T : class
    {
      public int status;
      public bool STATUS => status == 200;
      public string message;
      public T data;
    }
    public class emptyClass
    {

    }
}

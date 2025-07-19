using System;
using System.Web;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    internal class CookieWrapper : ICookie
    {
        private readonly HttpCookie cookie;

        public CookieWrapper(HttpCookie cookie)
        {
            this.cookie = cookie;
        }

        public string Domain
        {
            get => cookie.Domain;
            set => cookie.Domain = value;
        }

        public string Path
        {
            get => cookie.Path;
            set => cookie.Path = value;
        }

        public DateTimeOffset Expires
        {
            get => cookie.Expires == DateTime.MinValue ? DateTimeOffset.MinValue : new DateTimeOffset(cookie.Expires);
            set => cookie.Expires = value.UtcDateTime;
        }

        public bool Secure
        {
            get { 
                return cookie.Secure;
            }
            set { 
               cookie.Secure = value;
            }
        }

        public SameSiteEnum SameSite
        {
            get { 
                return (SameSiteEnum)cookie.SameSite;
            }
            set { 
                cookie.SameSite = (SameSiteMode)value;
            }
        }

        public bool HttpOnly
        {
            get => cookie.HttpOnly;
            set => cookie.HttpOnly = value;
        }

        public string Value => cookie.Value;

        public string Name => cookie.Name;
    }
}
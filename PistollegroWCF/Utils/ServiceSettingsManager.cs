using System;
using System.Configuration;


namespace PistollegroWCF.Utils
{
	public class PistollegroConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("thumbmailSuffix")]
		[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
		public String thumbmailSuffix
		{
			get
			{
				return (string)this["thumbmailSuffix"];
			}
			set
			{
				this["thumbmailSuffix"] = value;
			}
		}

		[ConfigurationProperty("picturesDirectory")]
		[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|")]
		public String picturesDirectory
		{
			get
			{
				return (string)this["picturesDirectory"];
			}
			set
			{
				this["picturesDirectory"] = value;
			}
		}

		[ConfigurationProperty("defaultPhotoFilename")]
		[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{};'\"|")]
		public String defaultPhotoFilename
		{
			get
			{
				return (string)this["defaultPhotoFilename"];
			}
			set
			{
				this["defaultPhotoFilename"] = value;
			}
		}


		[ConfigurationProperty("allowedPictureExtensions")]
		public string allowedPictureExtensions
		{
			get
			{
				return ((string)this["allowedPictureExtensions"]);
			}
			set
			{
				this["allowedPictureExtensions"] = value;
			}
		}

		[ConfigurationProperty("thumbmailSize")]
		public int thumbmailSize
		{
			get
			{
				return (int)this["thumbmailSize"];
			}
			set
			{
				this["thumbmailSize"] = value;
			}
		}

		public string[] pictureExtensionsList
		{
			get
			{
				return allowedPictureExtensions.Split(';');
			}
			private set { }
		}


	}
}

//namespace Pistollegro
//{
//	public class ServiceSettingsManager
//	{
//		NameValueCollection cm = ConfigurationManager.AppSettings;
//		public String thumbmailSuffix
//		{
//			get
//			{
//				return cm["thumbmailSuffix"];
//			}
//		}

//		public String picturesDirectory
//		{
//			get
//			{
//				return cm["picturesDirectory"];
//			}
//		}

//		public String defaultPhotoFilename
//		{
//			get
//			{
//				return cm["defaultPhotoFilename"];
//			}
//		}


//		public string allowedPictureExtensions
//		{
//			get
//			{
//				return cm["allowedPictureExtensions"];
//			}
//		}

//		[ConfigurationProperty("thumbmailSize")]
//		public int thumbmailSize
//		{
//			get
//			{
//				return Int32.Parse(cm["thumbmailSize"]);
//			}
//		}

//		public string[] pictureExtensionsList
//		{
//			get
//			{
//				return allowedPictureExtensions.Split(';');
//			}
//			private set { }
//		}


//	}
//}
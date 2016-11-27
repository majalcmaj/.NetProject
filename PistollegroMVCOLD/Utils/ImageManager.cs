using PistollegroMVC.Models;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;

namespace PistollegroMVC.Utils
{
	public class ImageManager
	{
		private HttpPostedFileBase weaponPicture;
		private int? weaponOfferID;
		private string photoDir;
		private ServiceSettingsManager config = (ServiceSettingsManager)ConfigurationManager.GetSection("pistollegroSettingsGroup/pistollegroSettings");
		public ImageManager(HttpPostedFileBase weaponPicture, string photoDir, int? weaponOfferID)
		{
			this.photoDir = photoDir;
			this.weaponPicture = weaponPicture;
			this.weaponOfferID = weaponOfferID;
		}

		public void SavePictureAndThumbmail()
		{
			if (weaponOfferID == null)
				return;
			Bitmap newImage = new Bitmap(config.thumbmailSize, config.thumbmailSize);
			using (Graphics gr = Graphics.FromImage(newImage))
			{
				gr.SmoothingMode = SmoothingMode.HighQuality;
				gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
				using (var stream = weaponPicture.InputStream)
				{
					var img = System.Drawing.Image.FromStream(stream);
					img.Save(System.IO.Path.Combine(photoDir, weaponOfferID.ToString() + ".png"), ImageFormat.Png);
					int height, width, positionX, positionY;
					CalculateThumbmailProperties(img, out height, out width, out positionX, out positionY);
					gr.DrawImage(img, new Rectangle(positionX, positionY, width, height));
					newImage.Save(System.IO.Path.Combine(photoDir, (weaponOfferID.ToString() + config.thumbmailSuffix + ".png")), ImageFormat.Png);
				}
			}
		}

		private void CalculateThumbmailProperties(Image img, out int height, out int width, out int positionX, out int positionY)
		{
			int thumbmailSideLength = config.thumbmailSize;
			height = img.Height;
			width = img.Width;
			positionX = 0;
			positionY = 0;
			if (height > width)
			{
				width = width * thumbmailSideLength / height;
				height = thumbmailSideLength;
				positionX = (thumbmailSideLength - width) / 2;
			}
			else
			{
				height = thumbmailSideLength * height / width;
				width = thumbmailSideLength;
				positionY = (thumbmailSideLength - height) / 2;
			}
		}
	}
}
namespace DiscogsNet.Model
{
    public class ImageAggregate
    {
        private Image image;

        public string Extension
        {
            get
            {
                int lastIndex = this.image.Uri.LastIndexOf('.');
                if (lastIndex == -1)
                {
                    return null;
                }
                return this.image.Uri.Substring(lastIndex);
            }
        }

        public string MimeType
        {
            get
            {
                switch (this.Extension.ToLower())
                {
                    case null:
                        return null;
                    case ".jpeg":
                    case ".jpg":
                        return "image/jpeg";
                    case ".png":
                        return "image/png";
                    case ".gif":
                        return "image/gif";
                    default:
                        return "application/unknown";
                }
            }
        }

        public ImageAggregate(Image image)
        {
            this.image = image;
        }
    }
}

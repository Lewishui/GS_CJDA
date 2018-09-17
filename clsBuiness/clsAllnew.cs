using clsdatabaseinfo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clsBuiness
{
    public class clsAllnew
    {
        private BackgroundWorker bgWorker1;
        //private object missing = System.Reflection.Missing.Value;
        public ToolStripProgressBar pbStatus { get; set; }
        public ToolStripStatusLabel tsStatusLabel1 { get; set; }
        public log4net.ILog ProcessLogger { get; set; }
        public log4net.ILog ExceptionLogger { get; set; }
        private System.Windows.Forms.PictureBox ptbNewPic;

        public List<clsFileNanme_info> FilelistResult;
        public List<string> Folder_list;

        public List<clsFileNanme_info> ReadFilelist(ref BackgroundWorker bgWorker)
        {

            string ZFCEPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\scan\\";
            FilelistResult = new List<clsFileNanme_info>();
            Folder_list = new List<string>();

            List<string> Alist1 = GetByfolderName(ZFCEPath);
            Folder_list = Alist1;

            for (int j = 0; j < Alist1.Count; j++)
            {
                List<string> Alist = GetFileName(Alist1[j] + "\\");
                for (int i = 0; i < Alist.Count; i++)
                {

                    clsFileNanme_info item = new clsFileNanme_info();
                    item.FilName = Alist[i].Replace(Alist1[j]+"\\", "");
                    item.Filpath = Alist[i];

                    item.foldername = Alist1[j].Replace(ZFCEPath, "");

                    FilelistResult.Add(item);

                }
            }
            // 

            return FilelistResult;

        }
        private List<string> GetFileName(string dirPath)
        {

            List<string> FileNameList = new List<string>();
            ArrayList list = new ArrayList();

            if (Directory.Exists(dirPath))
            {
                list.AddRange(Directory.GetFiles(dirPath));
            }
            if (list.Count > 0)
            {
                foreach (object item in list)
                {
                    if (!item.ToString().Contains("~$"))
                        FileNameList.Add(item.ToString().Replace(dirPath + "\\", ""));
                }
            }

            return FileNameList;
        }

        private List<string> GetByfolderName(string dirPath)
        {

            List<string> FileNameList = new List<string>();
            ArrayList list = new ArrayList();

            if (Directory.Exists(dirPath))
            {
                FileNameList.AddRange(Directory.GetDirectories(dirPath));
            }

            return FileNameList;

        }
        public void datatransfer(List<clsFileNanme_info> FilelistResult1)
        {

            FilelistResult = new List<clsFileNanme_info>();

            FilelistResult = FilelistResult;


        }

        public List<clsFileNanme_info> buinesschange(ref BackgroundWorker bgWorker)
        {
            try
            {

                foreach (clsFileNanme_info item in FilelistResult)
                {
                    ptbNewPic = new PictureBox();

                    //亮度
                    Bitmap b = new Bitmap(item.Filpath);
                    Bitmap bp = KiLighten(b, 12);
                    ptbNewPic.Image = bp;
                    //对比度

                    bp = KiContrast(bp, 20);
                    ptbNewPic.Image = bp;
                    int a = bp.Width;
                    int b0 = bp.Height;
                    ptbNewPic.Width = a;
                    ptbNewPic.Height = b0;
                    Save(item.FilName);

                }
                ///
                Folder_list = Folder_list.Distinct<string>().ToList();


                CreatePDF();


                return null;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static Bitmap KiLighten(Bitmap b, int degree)//亮度调节
        {
            if (b == null)
            {
                return null;
            }
            //确定最小值和最大值
            if (degree < -255) degree = -255;
            if (degree > 255) degree = 255;
            try
            {
                //确定图像的宽和高
                int width = b.Width;
                int height = b.Height;

                int pix = 0;
                //LockBits将Bitmap锁定到内存中
                BitmapData data = b.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    //p指向地址
                    byte* p = (byte*)data.Scan0;//8位无符号整数
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的亮度
                            for (int i = 0; i < 3; i++)
                            {
                                pix = p[i] + degree;
                                if (degree < 0) p[i] = (byte)Math.Max(0, pix);
                                if (degree > 0) p[i] = (byte)Math.Min(255, pix);
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                b.UnlockBits(data);//从内存中解除锁定

                return b;
            }
            catch
            {
                return null;
            }

        }

        public static Bitmap KiContrast(Bitmap b, int degree)//对比度调节
        {
            if (b == null)
            {
                return null;
            }

            if (degree < -100) degree = -100;
            if (degree > 100) degree = 100;

            try
            {

                double pixel = 0;
                double contrast = (100.0 + degree) / 100.0;
                contrast *= contrast;
                int width = b.Width;
                int height = b.Height;
                BitmapData data = b.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的对比度
                            for (int i = 0; i < 3; i++)
                            {
                                pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                                if (pixel < 0) pixel = 0;
                                if (pixel > 255) pixel = 255;
                                p[i] = (byte)pixel;
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                b.UnlockBits(data);
                return b;
            }
            catch
            {
                return null;
            }
        }

        private void Save(string picPath)
        {

            string ZFCEPath = AppDomain.CurrentDomain.BaseDirectory + "Results\\";
            picPath = ZFCEPath + picPath;
            Graphics g = ptbNewPic.CreateGraphics();
            Bitmap bt1 = new Bitmap(ptbNewPic.Image);
            Bitmap mybmp1 = new Bitmap(bt1, ptbNewPic.Width, ptbNewPic.Height);
            mybmp1.Save(picPath, ImageFormat.Jpeg);

        }

        private void CreatePDF()
        {
            for (int i = 0; i < Folder_list.Count; i++)
            {
                string ZFCEPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\scan\\";

                string outMergeFile = AppDomain.CurrentDomain.BaseDirectory + "Results\\" + Folder_list[i].Replace(ZFCEPath,"") + "_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                List<clsFileNanme_info> cloumnlist = FilelistResult.FindAll(s => s.foldername != null && s.foldername == Folder_list[i].Replace(ZFCEPath, ""));
                ImageDirect(outMergeFile, cloumnlist);
            }
        }
        public void ImageDirect(string savename, List<clsFileNanme_info> FilelistResult)
        {
            string imagePath = AppDomain.CurrentDomain.BaseDirectory + @"Image\1.jpg"; //临时文件路径
            string fileName = string.Empty;

            {
                fileName = savename;
                Document document = new Document();
                iTextSharp.text.Rectangle page = PageSize.A4;
                float y = page.Height;
                document = new Document(page, 15, 15, 30, 30);
                float docWidth = page.Width - 15 * 2;
                float docHeight = page.Height - document.BottomMargin - document.TopMargin;

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                document.Open();

                foreach (clsFileNanme_info item in FilelistResult)
                {

                    iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(item.Filpath);
                    float widthSzie = (page.Width - 30) / img1.Width;
                    if (widthSzie < 1)
                    {
                        img1.ScalePercent(widthSzie * 100);
                    }
                    document.Add(img1);
                }

                document.Close();
            }
        }
    }
}

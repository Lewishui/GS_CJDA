using clsBuiness;
using clsCommon;
using clsdatabaseinfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace GS_CJDA
{
    public partial class frmMainImage : DockContent
    {
        public List<string> listfile = new List<string>();
        public string selectedImage = string.Empty;
        public string selectedImageAbsolutePath = string.Empty;
        List<clsFileNanme_info> listpage;
        public string strFileName = String.Empty;

        // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        public List<clsFileNanme_info> FilelistResult;
        public List<string> Folder_list;



        public frmMainImage()
        {
            InitializeComponent();
        }
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                blnBackGroundWorkIsOK = false;
            }
            else if (e.Cancelled)
            {
                blnBackGroundWorkIsOK = true;
            }
            else
            {
                blnBackGroundWorkIsOK = true;
            }
        }
        private void InitialBackGroundWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (frmMessageShow != null && frmMessageShow.Visible == true)
            {
                //设置显示的消息
                frmMessageShow.setMessage(e.UserState.ToString());
                //设置显示的按钮文字
                if (e.ProgressPercentage == clsConstant.Thread_Progress_OK)
                {
                    frmMessageShow.setStatus(clsConstant.Dialog_Status_Enable);
                }
            }
        }

        private void 读取_Click(object sender, EventArgs e)
        {
            try
            {


                InitialBackGroundWorker();
                Control.CheckForIllegalCrossThreadCalls = false;
                bgWorker.DoWork += new DoWorkEventHandler(Getmytask);

                bgWorker.RunWorkerAsync();
                // 启动消息显示画面
                frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                    clsShowMessage.MSG_007,
                                                    clsConstant.Dialog_Status_Disable);
                frmMessageShow.ShowDialog();
                // 数据读取成功后在画面显示
                if (blnBackGroundWorkIsOK)
                {
                    // this.dataGridView.DataSource = null;
                    //this.dataGridView.AutoGenerateColumns = false;
                    //sortableCaseListResult = new SortableBindingList<clsAP_fuwuinfo>(CaseListResult);
                    //this.dataGridView.DataSource = sortableCaseListResult;
                    if (FilelistResult != null && FilelistResult.Count > 0)
                    {
                        listBox1.Items.Clear();

                        foreach (clsFileNanme_info item in FilelistResult)
                            this.listBox1.Items.Add(item.FilName);
                        this.tabControl1.SelectedIndex = 1;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void Getmytask(object sender, DoWorkEventArgs e)
        {
            FilelistResult = new List<clsFileNanme_info>();
            Folder_list = new List<string>();

            //初始化信息
            clsAllnew BusinessHelp = new clsAllnew();
            //BusinessHelp.pbStatus = pbStatus;
            //BusinessHelp.tsStatusLabel1 = toolStripLabel2;
            DateTime oldDate = DateTime.Now;
            FilelistResult = BusinessHelp.ReadFilelist(ref this.bgWorker);
            Folder_list = BusinessHelp.Folder_list;


            DateTime FinishTime = DateTime.Now;  //   
            TimeSpan s = DateTime.Now - oldDate;
            string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
            string Showtime = clsShowMessage.MSG_029 + timei.ToString();
            bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_009 + "\r\n" + Showtime);
        }

        private void shellView1_Click(object sender, EventArgs e)
        {

        }

        private void shellView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.shellView1.SelectedItems.Count() == 1)
            {
                GetSelectImagePath();
                //  this.Close();
            }
        }
        private string GetSelectImagePath()
        {
            string imageRelativePath = string.Empty;

            if (this.shellView1.SelectedItems.Count() > 0)
            {
                var item = this.shellView1.SelectedItems.First();
                for (int ii = 0; ii < this.shellView1.SelectedItems.Count(); ii++)
                {
                    var kk = shellView1.SelectedItems[ii];

                    string absolutePath = kk.FileSystemPath;

                    this.selectedImageAbsolutePath = absolutePath;
                    clsFileNanme_info itemn = new clsFileNanme_info();

                    itemn.FilName = kk.DisplayName;
                    itemn.Filpath = selectedImageAbsolutePath;
                    this.listBox1.Items.Add(itemn.FilName);

                    FilelistResult.Add(itemn);


                }
                //  this.selectedImage = absolutePath.Substring(EntityPathHelper.ImageBasePath.Length);
            }
            return imageRelativePath;

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            FilelistResult = new List<clsFileNanme_info>();

            GetSelectImagePath();
            this.tabControl1.SelectedIndex = 1;

            //  this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {


                InitialBackGroundWorker();
                Control.CheckForIllegalCrossThreadCalls = false;
                bgWorker.DoWork += new DoWorkEventHandler(changePIC);

                bgWorker.RunWorkerAsync();
                // 启动消息显示画面
                frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                    clsShowMessage.MSG_007,
                                                    clsConstant.Dialog_Status_Disable);
                frmMessageShow.ShowDialog();
                // 数据读取成功后在画面显示
                if (blnBackGroundWorkIsOK)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void changePIC(object sender, DoWorkEventArgs e)
        {
            // FilelistResult = new List<clsFileNanme_info>();

            //初始化信息
            clsAllnew BusinessHelp = new clsAllnew();
            //BusinessHelp.pbStatus = pbStatus;
            //BusinessHelp.tsStatusLabel1 = toolStripLabel2;
            DateTime oldDate = DateTime.Now;

            BusinessHelp.FilelistResult = FilelistResult;
            BusinessHelp.Folder_list = Folder_list;
          
            
            BusinessHelp.buinesschange(ref this.bgWorker);

            DateTime FinishTime = DateTime.Now;  //   
            TimeSpan s = DateTime.Now - oldDate;
            string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
            string Showtime = clsShowMessage.MSG_029 + timei.ToString();
            bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_009 + "\r\n" + Showtime);
        }

    }
}

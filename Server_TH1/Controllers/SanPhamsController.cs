using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Server_TH1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SanPhamsController : ApiController
    {
        static String connectionString = "Data Source=DUNGNGUYEN;Initial Catalog = DuLieu; Integrated Security = True";

        // Lấy thông tin toàn bộ sản phẩm
        [HttpGet]
        public List<tblSanPham> GetAllSanPham()
        {
            DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
            return db.tblSanPhams.ToList();
        }

        // Lấy thông tin của các sản phẩm theo tên gần sản phẩm (vơi tìm kiếm gần đúng)
        [Route("Search/{key}")]
        [HttpGet]
        public List<tblSanPham> GetSanPham(String key)
        {
            DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
            //String key1 = "/"+key+"/";
            List<tblSanPham> result = db.tblSanPhams.Where(x => x.TenSP.StartsWith(key)).ToList();
            return result;
        }

        // Lấy danh sách sản phẩm tồn kho (với số lượng > 0)
        [Route("TonKho")]
        [HttpGet]
        public List<tblSanPham> GetSanPhamTonKho()
        {
            DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
            List<tblSanPham> result = db.tblSanPhams.Where(x => x.SoLuong > 0).ToList();
            return result;
        }

        // Thêm sản phẩm vào csdl

        [HttpPost]
        public bool PostSanPham(string MaSP, string TenSP, string MoTa, decimal GiaBan, decimal GiaNhap, int SoLuong)
        {
            try
            {
                DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
                tblSanPham newSP = new tblSanPham();
                newSP.MaSP = MaSP;
                newSP.TenSP = TenSP;
                newSP.MoTa = MoTa;
                newSP.GiaBan = GiaBan;
                newSP.GiaNhap = GiaNhap;
                newSP.SoLuong = SoLuong;

                db.tblSanPhams.InsertOnSubmit(newSP);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sửa một sản phẩm

        [HttpPut]
        public bool Update(string MaSP, string TenSP, string MoTa, decimal GiaBan, decimal GiaNhap, int SoLuong)
        {
            try
            {
                DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
                tblSanPham product = db.tblSanPhams.FirstOrDefault(n => n.MaSP == MaSP);
                if (product == null) { return false; }
                product.MaSP = MaSP;
                product.TenSP = TenSP;
                product.MoTa = MoTa;
                product.GiaBan = GiaBan;
                product.GiaNhap = GiaNhap;
                product.SoLuong = SoLuong;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        // Xóa một sản phẩm
        [HttpDelete]
        public bool DeleteSanPham(string MaSP)
        {
            try
            {
                DBSanPhamDataContext db = new DBSanPhamDataContext(connectionString);
                //Lấy mã sản phẩm đã có
                tblSanPham sp = db.tblSanPhams.FirstOrDefault(x => x.MaSP == MaSP);

                if (sp == null) return false;

                db.tblSanPhams.DeleteOnSubmit(sp);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        

    }
}

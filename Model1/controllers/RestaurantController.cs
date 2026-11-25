using Microsoft.AspNetCore.Mvc;
using Model1.Models;

namespace Model1.Controllers
{
    // ระบุว่าคลาสนี้เป็น Web API Controller (ช่วยจัดการเรื่องการรับ-ส่งข้อมูลและตรวจสอบความถูกต้องเบื้องต้น)
    [ApiController]
    // สร้าง Controller ชื่อ RestaurantController และรับตัวแปร db (MapDbContext) เพื่อใช้เชื่อมต่อฐานข้อมูล
    public class RestaurantController(MapDbContext db) : ControllerBase
    {
        // กำหนดเส้นทาง (URL) สำหรับเรียกใช้งาน API นี้ด้วยเมธอด GET
        [HttpGet("system/api/restaurant/list")]
        public ApiResult List(
            int pageNam = 1,               // รับค่าเลขหน้า (ถ้าไม่ส่งมา จะเริ่มที่หน้า 1) *หมายเหตุ: น่าจะตั้งใจพิมพ์ว่า pageNum
            int pageSize = 10,             // รับค่าจำนวนรายการต่อหน้า (ถ้าไม่ส่งมา จะดึง 10 รายการ)
            string? restaurantName = null, // รับค่าชื่อร้านอาหารสำหรับค้นหา (ไม่ส่งมาก็ได้)
            string? restaurantType = null) // รับค่าประเภทร้านอาหารสำหรับกรองข้อมูล (ไม่ส่งมาก็ได้)
        {
            // เตรียมคำสั่งสำหรับดึงข้อมูลจากตาราง Restaurants (ยังไม่ดึงข้อมูลจริง จนกว่าจะสั่ง ToList หรือ Count)
            var q = db.Restaurants.AsQueryable(); 

            // ตรวจสอบว่ามีการส่ง "ชื่อร้านอาหาร" มาค้นหาหรือไม่
            if (!string.IsNullOrEmpty(restaurantName))
                // กรองเฉพาะร้านที่มีชื่อตรงกับคำค้นหา (คล้ายคำสั่ง LIKE '%ชื่อร้าน%' ใน SQL)
                q = q.Where(x => x.RestaurantName!.Contains(restaurantName));

            // ตรวจสอบว่ามีการส่ง "ประเภทร้านอาหาร" มาหรือไม่
            if (!string.IsNullOrEmpty(restaurantType))
                // กรองเฉพาะร้านที่มีประเภทตรงกันเป๊ะๆ (คล้ายคำสั่ง = 'ประเภทร้าน' ใน SQL)
                q = q.Where(x => x.RestaurantType == restaurantType);

            // ส่งผลลัพธ์กลับไปว่าทำงานสำเร็จ
            // *ข้อสังเกต: โค้ดส่วนนี้ยังไม่ได้สั่งดึงข้อมูลจริง (เช่น q.ToList()) และยังไม่ได้นำ pageNam, pageSize มาแบ่งหน้า รวมถึงยังไม่ได้แนบข้อมูลกลับไปใน ApiResult.Ok()
            return ApiResult.Ok();
        }
    }
}
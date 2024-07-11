using AutoMapper;
using HabVilla_CouponAPI;
using HabVilla_CouponAPI.Data;
using HabVilla_CouponAPI.Models;
using HabVilla_CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("/helloworld", () =>
//{
//    return Results.BadRequest("Exception!!!");
//});

//app.MapGet("/helloworld{int:id}", (int id) =>
//{
//    return Results.Ok("id!!!!" + id);
//});

//app.MapGet("/helloworld", () =>
//{
//    return "hello world";
//});

//app.MapPost("/helloworld2", () => "Hello World 2");

//CRUD OPERATIONS ON COUPONS..........................
app.MapGet("/api/coupon", (ILogger<Program> _logger)=> {
    _logger.Log(LogLevel.Information, "Getting all Coupons");
    return Results.Ok(CouponStore.couponList);
    }).WithName("GetCoupons").Produces<IEnumerable<Coupon>>(200);

app.MapGet("/api/coupon/{id:int}", (int id) => {
    return Results.Ok(CouponStore.couponList.FirstOrDefault(u => u.Id == id));
}).WithName("GetCoupon").Produces<Coupon>(200);

app.MapPost("/api/coupon", (IMapper _mapper,[FromBody] CouponCreateDTO coupon_C_DTO) => {
    if (string.IsNullOrEmpty(coupon_C_DTO.Name))
    {
        return Results.BadRequest("Invalid Id or Coupon Name!");
    }
    if (CouponStore.couponList.Any(u => u.Name.ToLower() == coupon_C_DTO.Name.ToLower()))
    {
        return Results.BadRequest("Coupon Name already exists!");
    }
    Coupon coupon = new()
    {
        Name = coupon_C_DTO.Name,
        Percent = coupon_C_DTO.Percent,
        IsActive = coupon_C_DTO.IsActive
    };
    var maxId = CouponStore.couponList.Any() ? CouponStore.couponList.Max(u => u.Id) : 0;
    coupon.Id = maxId + 1;
    CouponStore.couponList.Add(coupon);

    CouponDTO couponDTO = new()
    {   Id = coupon.Id,
        Name = coupon.Name,
        Percent = coupon.Percent,
        IsActive = coupon.IsActive,
        Created = coupon.Created
    };
    return Results.CreatedAtRoute("GetCoupon", new {id = coupon.Id}, coupon);
    //return Results.Created($"/api/coupon/{coupon.Id}",coupon);
}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<CouponDTO>(201).Produces<Coupon>(400);


app.MapPut("/api/coupon", () => {
    
});

app.MapDelete("/api/coupon/{id:int}", (int id) => {
    
});

app.UseHttpsRedirection();

app.Run();

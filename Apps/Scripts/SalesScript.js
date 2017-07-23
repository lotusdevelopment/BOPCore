var saleObj = {};
var bens = 0;
var same = true;
var valC = false;
var latamPlans;
var familiar = false;
var anual = false;
var max = 1;
var existingBens = 0;
var countries = [];
var plans = [];
var currentPlan = {};

$(document).ready(function () {
    // $('select').material_select();
   
});

$(function () {
    GraphInitials();
      $('#btnStep1').click(function () {
        saleObj.CountryId = 1;
        saleObj.Plan = $('#sltNewProducts').val();
        saleObj.PlanText = $('#sltNewProducts').find(':selected').data('infor');
        max = parseInt($('#sltPType').find(':selected').data('type'));
        $('.steps-former .atr1').removeClass("ls-active");
        $('.steps-former .atr1').addClass("ls-past");
        $('.steps-former .atr2').addClass("ls-active");
        if (max > 1) {
            $('.incld').text('* Incluir beneficiarios (' + max + ')');
            $('.incld').show();
        }
        $(".atr1").removeClass("active ls-active");
        $(".atr1").addClass("ls-past");
        $(".atr2").addClass("active ls-active");
        $("#Stp1").removeClass("active in");
        $("#Stp2").addClass("active in");
        $(".bar").css("width", "66.6667%");
        GraphStep2();
    });

    
      $('#btnStep2').click(function () {
        /*  var full = true;
          var myphone = $('#Mobile').val().replace(/\D/g, '');

         
          if (!full) {
              swal(
                  'Please',
                  'Fill all the mandatory fields',
                  'error'
              );
              return false;
          }

          if ($('#Mobile').val().length < 6 && $('#Mobile').val().length > 1) {
              $('#Mobile').addClass("invalid");
              swal(
                  'Please',
                  'Enter a valid mobile Phone',
                  'error'

              );

              return false;
          }

          if (myphone.length < 6 && myphone.length > 0) {
              swal(
                  'Please',
                  'The number must 7+ digits',
                  'error'
              );
              console.log("este fue el alert del mobile");
              return false;
          }
          if ($('#Same').val() != "0" && $('#Same').val() != "1") {
              $('#Same').addClass("invalidA");
              return false;
          }
          if ($('#Name').val().length < 1) {
              $('#Name').addClass("invalid");
              return false;
          }
          if ($('#LastName').val().length < 1) {
              $('#LastName').addClass("invalid");
              return false;
          }
          $('#Same').removeClass("invalidA");
          if ($('#Same').val() === "1") {
              if ($('#Email').val().length < 1) {
                  swal(
                      'Please',
                      'Enter an E-mail!',
                      'error'
                  );

                  return false;
              }
              if (!validateEmail($('#Email').val())) {
                  swal(
                      'Please',
                      'Enter a valid Email',
                      'error'
                  );

                  return false;
              }
              same = true;
          }*/
          $('.selectium').each(function (i, j) {
              if (IsNullOrEmpty(j.value)) {
                  full = false;
                  $('#' + j.id).addClass("invalid");
              } else $('#' + j.id).addClass("valid");
          });
          saleObj.FirstName = $('#Name').val();
          saleObj.LastName = $('#LastName').val();
          saleObj.CustomerId = $('#cId').val();
          saleObj.Cellphone = myphone;
          saleObj.Email = $('#Email').val();
          saleObj.Street = $('#Address').val();
          saleObj.Birthday = $('#Birthday').val();
          saleObj.Gender = "Male";
          if (existingBens > 0) {
              var beints = [];
              for (var k = 0; k < existingBens; k++) {
                  beints.push({ Name: $("#Name" + k).val(), Lastname: $("#LastName" + k).val(), Id: $("#cId" + k).val() });
              }
              saleObj.Beneficiaries = beints;
          }
          $(".atr2").removeClass("active ls-active");
          $(".atr2").addClass("ls-past");
          $(".atr3").addClass("active ls-active");
          $("#Stp2").removeClass("active in");
          $("#Stp3").addClass("active in");
          $(".bar").css("width", "100%");
          GraphStep3();
      });

    $('#btnStep3').click(function () {
        var ver = true;
        $('.validCarda').each(function (i, j) {
            if ($(this)[0].value.length < 1) {
                ver = false;
                $(this).addClass("invalid");
            }
        });
        if (!ver)
            return;
        if (saleObj.PaymentType === 0) {
            if (!valC) {
                //alert('Invalid Card');
                //return false;
            }
            ver = true;
            $('.validCard').each(function (i, j) {
                if ($(this)[0].value.length < 1) {
                    ver = false;
                    $(this).addClass("invalid");
                }
            });
            if (!ver) {
                swal(
                    'Please',
                    'Fill all the mandatory fields',
                    'error'
                );
                return;
            }
            saleObj.CardData = {
                CardName: $('#CH').val(), CardNumber: $('#CN').val(), CardCvc: $('#CV').val(),
                CardExpirationMonth: $('#EM').val(), CardExpirationYear: $('#EY').val()
            };
        }
        saleObj.BuyerName = $('#BuyerName').val();
        saleObj.BuyerCellPhone = $('#BuyerPhone').val();
        saleObj.BuyerEmail = $('#BuyerMail').val();
        GoBuy();
        return;
    });
    $('#btnGetProducts').click(function () {
        $("#ProductsList").empty();
        $(".products-container").show();
        $.ajax({
            url: '/Others/GetNewProducts',
            type: "POST",
            data: {
                pType: $("#sltPType").val(), vType: $("#sltVTime").val(),
                pPeriod: $("#sltPPeriod").val(), curr: $("#sltCurrencie").val()
            },
            cache: true,
            success: function (data) {
                if (data.length < 1) {
                    $("#ProductsList").html('<label>No available products for these configurations.</label>');
                    $("#btnStep1").hide();
                    return;
                } else {
                    var d = $lf.parseString(data);
                    if (d.Error.IsError || d.Products.length < 1) {
                        $("#ProductsList").html('<label>No available products for these configurations.</label>');
                        $("#btnStep1").hide();
                        return;
                    }
                    $("#ProductsList").html('<h5 class="title">Select the product</h5><select id="sltNewProducts" class="form-control"></select>');
                    plans = d.Products;
                    $.each(d.Products, function (i, j) {
                        $("#sltNewProducts").append('<option value="' + j.ProductId + '" data-infor="' + j.ProductName + ' - $' + j.ProductPrice + ' ' + j.Currency + '">' + j.ProductName + ' - $' + j.ProductPrice + ' ' + j.Currency + '</option>');
                    });
                    $("#btnStep1").show();
                }
            },
            error: function (xhr) {
                debugger;
                $("#ProductsList").html('<label>No available products for these configurations.</label>');
                $("#btnStep1").hide();
                return;
            }
        });
    });
    $("input[name='groupP']").change(function () {
        $('.payment').each(function (i, j) {
            $(this).removeClass("visible");
        });
        $('.' + $(this)[0].id).addClass("visible");
        saleObj.PaymentType = parseInt($(this)[0].value);
    });
    $('.selectium').change(function () {
        if ($(this).hasClass("invalid")) {
            $(this).removeClass("invalid");
            $(this).addClass("valid");
        } else if ($(this).hasClass("invalidA")) {
            $(this).removeClass("invalidA");
        }
    });
    $('.validCarda').change(function () {
        if ($(this).hasClass("invalid")) {
            $(this).removeClass("invalid");
            $(this).addClass("valid");
        }
    });
    $('.validCard').change(function () {
        if ($(this).hasClass("invalid")) {
            $(this).removeClass("invalid");
            $(this).addClass("valid");
        }
    });
    HideModal2();
});



function GraphInitials() {
    $('.steps-former .atr1').addClass("ls-active");
    $(".products-container").hide();
    //GetAndGraphCountries();
    GetAndGraphPayment();
    GetAndGraphGenerals();
}

function GetAndGraphGenerals() {
    if (localStorage.generalSelects != null) {
        var d = $lf.parseString(localStorage.generalSelects);
        countries = d.Countries;
        GraphValidityTime(d.ValidityTime);
        GraphPaymentPeriod(d.PaymentPeriod);
        GraphCurrencies(d.CurrencyProd);
        GraphInfoContract(d.InfoContractProd);
        return;
    }
    $.ajax({
        url: '/Others/GetGenerals',
        type: "POST",
        cache: true,
        success: function (data) {
            if (data.length < 1) {
                alert('Error');
                return;
            } else {
                localStorage.removeItem("generalSelects");
                var d = $lf.parseString(data);
                localStorage.setItem('generalSelects', $lf.parseObject(d));
                countries = d.Countries;
                GraphValidityTime(d.ValidityTime);
                GraphPaymentPeriod(d.PaymentPeriod);
                GraphCurrencies(d.CurrencyProd);
                GraphInfoContract(d.InfoContractProd);
            }
        },
        error: function (xhr) {
            alert('Error.');
            saleObj = {};
        }
    });
}

function GraphInfoContract(d) {
    $.each(d, function (i, j) {
        $("#sltPType").append('<option value="' + j.Id + '" data-type="' + j.LifesQuantity + '">' + j.Description + '</option>');
    });
}

function GraphCurrencies(d) {
    $.each(d, function (i, j) {
        $("#sltCurrencie").append('<option value="' + j.Id + '">' + j.Description + '</option>');
    });
}

function GraphPaymentPeriod(d) {
    $.each(d, function (i, j) {
        $("#sltPPeriod").append('<option value="' + j.Id + '">' + j.Description + '</option>');
    });
}

function GraphValidityTime(d) {
    $.each(d, function (i, j) {
        $("#sltVTime").append('<option value="' + j.Id + '">' + j.Description + '</option>');
    });
}

function GetAndGraphCountries() {
    $.ajax({
        url: '/Others/GetCountries',
        type: "GET",
        cache: true,
        success: function (data) {
            if (data.length < 1) {
                alert('Error cargando la lista de paises, por favor recargue la página');
                return false;
            } else {
                try {
                    if (data.Countries.length < 1) {
                        alert('Error cargando la lista de paises, por favor recargue la página');
                        return false;
                    }
                } catch (e) {
                }
                console.log("Paises");
                $(data).each(function (i, j) {
                    console.log(i, j);
                    var opt1 = '<option value="' + j.CountryId + '">' + j.Country + '</option>';
                    $('#sltCountry').append(opt1);
                });
                $("#Cntr").fadeIn("slow");
                $('#sltCountry').change(function () {
                    //ShowCortinilla();
                    GetAndGraphPlans();
                });
            }
            return true;
        },
        error: function (xhr) {
            debugger;
            alert('Hubo error en el tiempo de espera, por favor vuelva a intentarlo.');
            saleObj = {};
            //
        }
    });
}

function GetAndGraphPlans() {
    $('#latP').empty();
    $('#plny').empty();
    var country = parseInt($('#sltCountry').val());
    if (country === 0) {
        $('#Plans').empty();
        $("#plny").fadeOut("slow");
        $('#btnStep1').fadeOut("slow");
        $('#latP').empty();
    } else {
        var val = $('#sltCountry option:selected').text().toUpperCase();
        if (val !== "LATAM" && val !== "LAT" && val !== "LATINOAMÉRICA") {
            if (val !== "CRC" && val !== "COSTA" && val !== "COSTA RICA")
                GetPlans(country);
            else
                GetPlansCosta(country);
        } else
            DrawAndPlayLatam();
    }
}

function GetPlansCosta(country) {
    $('#latP').append('<h5>Selecciona el tipo de plan</h5>' +
        '<select id="CrTypeM" class="form-control">' +
        '<option value="0" selected>Selecciona una opción</option>' +
        '<option value="true">Anual</option>' +
        '<option value="false">Mensual</option>' +
        '</select>' +
        '<div id="pmDv"></div>');
    $('#CrTypeM').change(function () {
        $('#pmDv').empty();
        GetAndDrawCR(country, $('#CrTypeM').val());
    });
}

function DrawAndPlayLatam() {
    $('#latP').append('<h5>Selecciona el tipo de plan</h5>' +
        '<select id="FamType" class="form-control">' +
        '<option value="0" selected>Selecciona una opción</option>' +
        '<option value="false">Individual</option>' +
        '<option value="true">Familiar</option>' +
        '</select>' +
        '<div id="pmDv"></div>');
    //HideCortinilla();
    $('#FamType').change(function () {
        $('#pmDv').empty();
        $('#plny').empty();
        $('#pmDv').append('<h5>Selecciona el periodo de pago</h5>' +
            '<select id="PmntTp" class="form-control">' +
            '<option value="0" selected>Selecciona una opción</option>' +
            '<option value="false">Mensual</option>' +
            '<option value="true">Anual</option>' +
            '</select>');
        $('#PmntTp').change(function () {
            $('#plny').empty();
            GetAndDrawLatam();
        });
    });
}

function GetAndDrawLatam() {
    //ShowCortinilla();
    familiar = $('#FamType').val();
    anual = $('#PmntTp').val();
    $.ajax({
        url: '/Sales/GetPlansLatam?familiar=' + familiar + '&anual=' + anual,
        type: "GET",
        cache: true,
        success: function (data) {
            $('#Plans').empty();
            $("#plny").fadeOut("slow");
            //$('#btnStep1').fadeOut("slow");
            if (data.length < 1) {
                alert('No existen planes para este país, por favor seleccione otro país e intente de nuevo');
            } else {
                $('#plny').html('<h5>Elige un plan</h5><select id="Plans" class="form-control"></select>');
                console.log("Planes");
                var opt1 = '<option value="0" disabled selected>Selecciona un plan</option>';
                $('#Plans').append(opt1);
                $(data).each(function (i, j) {
                    console.log(i, j);
                    opt1 = '<option value="' + j.PlanId + '" data-desc="' + j.Description + '" data-currency="' + j.Currency + '" ' +
                        'data-price="' + j.Value + '">' + j.PlanName + '</option>';
                    $('#Plans').append(opt1);
                });
                $("#plny").fadeIn("slow");
                $('#btnStep1').removeClass('hidden');
            }
            //HideCortinilla();
        },
        error: function (xhr) {
            debugger;
            alert('Hubo error en el tiempo de espera, por favor vuelva a intentarlo.');
            saleObj = {};
            //
        }
    });
}

function GetAndDrawCR(country, type) {
    anual = type;
    $.ajax({
        url: '/Sales/GetLatamCr?country=' + country + '&anual=' + anual,
        type: "GET",
        cache: true,
        success: function (data) {
            $('#Plans').empty();
            $("#plny").fadeOut("slow");
            //$('#btnStep1').fadeOut("slow");
            if (data.length < 1) {
                alert('No existen planes para este país, por favor seleccione otro país e intente de nuevo');
            } else {
                $('#plny').html('<h5>Elige un plan</h5><select id="Plans" class="form-control"></select>');
                console.log("Planes");
                var opt1 = '<option value="0" disabled selected>Selecciona un plan</option>';
                $('#Plans').append(opt1);
                $(data).each(function (i, j) {
                    console.log(i, j);
                    opt1 = '<option value="' + j.PlanId + '" data-desc="' + j.Description + '" data-currency="' + j.Currency + '" ' +
                        'data-price="' + j.Value + '">' + j.PlanName + '</option>';
                    $('#Plans').append(opt1);
                });
                $("#plny").fadeIn("slow");
                $('#btnStep1').removeClass('hidden');
            }
        },
        error: function (xhr) {
            debugger;
            alert('Hubo error en el tiempo de espera, por favor vuelva a intentarlo.');
            saleObj = {};
        }
    });
}

function GetAndGraphPayment() {
    /*$.ajax({
        url: '/Sales/GetPf',
        type: "GET",
        cache: true,
        success: function (data) {
            if (data.length < 1) {
                alert('Usted no posee formas de pago asociadas, no puede realizar la venta');
                //
            } else {
                GraphPm(data);
            }
        },
        error: function (xhr) {
            debugger;
            alert('Usted no posee formas de pago asociadas, no puede realizar la venta o hubo un error al obtenerlas');
            //
        }
    });*/
    data = ["TARJETA DE CREDITO", "TARJETA DE CREDITO (PREVENTA)"];
    GraphPm(data);
}

function GraphPm(data) {
    $(data).each(function (i, j) {
        if (j === "EFECTIVO") {
            $('.pmntO').append('<div class="col-lg-6"><div class="radio radio-info"><input name="groupP" type="radio" id="Cashe" checked value="1" /><label for="Cashe">Efectivo</label></div><div>');
            $('.pmntI').append('<div class="col-lg-12 Cashe payment visible"><span>El método de pago escogido es efectivo, recuerde recibir el valor total de la transacción.</span></div>');
        }
        if (j === "TARJETA DE CREDITO") {
            $('.pmntO').append('<div class="col-lg-6"><div class="radio radio-info"><input name="groupP" type="radio" id="Carde" value="0" /><label for="Carde">Credit Card</label></div></div>');
            var tc = '<div class="col-lg-12 Carde payment">'
                + '<h5 style="text-align:center;color:green">Fill the following information of the credit card.</h5>'
                + '<div class="form-group col-lg-6">'
                + '<div class="col-lg-12">'
                + '<input id="CN" type="text" class="validCard form-control" name="card_number" placeholder="Card Number">'
                + '</div>'
                + '</div>'
                + '<div class="form-group col-lg-6">'
                + '<div class="col-lg-12">'
                + '<input id="CH" type="text" class="validCard form-control" placeholder="Cardholder">'
                + '</div>'
                + '</div>'
                + '<div class="form-group col-lg-4">'
                + '<div class="col-lg-12">'
                + '<input id="CV" type="number" class="validCard form-control" min="000" max="9999" maxlength="4" placeholder="CVV (*)">'
                + '</div>'
                + '</div>'
                + '<div class="form-group col-lg-4">'
                + '<select id="EM" class="validCard form-control">'
                + '<option disabled selected>Expiration month (*)</option>'
                + '<option value="01">January</option>'
                + '<option value="02">February</option>'
                + '<option value="03">March</option>'
                + '<option value="04">April</option>'
                + '<option value="05">May</option>'
                + '<option value="06">June</option>'
                + '<option value="07">July</option>'
                + '<option value="08">August</option>'
                + '<option value="09">September</option>'
                + '<option value="10">October</option>'
                + '<option value="11">November</option>'
                + '<option value="12">December</option>'
                + '</select>'
                + '</div>'
                + '<div class="form-group col-lg-4">'
                + '<select id="EY" class="validCard form-control">'
                + '<option disabled selected>Expiration Year (*)</option>'
                + '</select>'
                + '</div>'
                + '</div>';
            $('.pmntI').append(tc);
            var currentYear = (new Date).getFullYear();
            for (var k = 0; k < 10; k++) {
                var maxY;
                if (k === 0)
                    maxY = currentYear;
                else
                    maxY = parseInt(currentYear) + (k + 1);
                $('#EY').append('<option value="' + maxY + '">' + maxY + '</option>');
            }
        }
        if (j === "TARJETA DE CREDITO (PREVENTA)") {
            $('.pmntO').append('<div class="col-lg-6"><div class="radio radio-info"><input name="groupP" type="radio" id="CardePref" value="2" /><label for="CardePref">Credit Card (Presale)</label></div></div>');
            $('.pmntI').append('<div class="col-md-12 CardePref payment" style="text-align:center"><span>The selected payment is Credit card (Presale). The customer will receive an email with a link in order to make the payment.</span></div>');
        }
    });
    $("input[name='groupP']").change(function () {
        $('.payment').each(function (i, j) {
            $(this).removeClass("visible");
        });
        $('.' + $(this)[0].id).addClass("visible");
        saleObj.PaymentType = parseInt($(this)[0].value);
    });
    $('#CN').keyup(function () {
        $('#CN').validateCreditCard(function (result) {
            valC = result.valid;
            //debugger;
            if (valC) {
                //debugger;
                var imgUrl = '../../Content/Images/others.png';
                switch (result.card_type.name) {
                    case "visa":
                        imgUrl = '../../Content/Images/visa.png';
                        break;
                    case "amex":
                        imgUrl = '../../Content/Images/amex.png';
                        break;
                    case "diners_club_international":
                        imgUrl = '../../Content/Images/diners.png';
                        break;
                    case "diners_club_carte_blanche":
                        imgUrl = '../../Content/Images/diners.png';
                        break;
                    case "visa_electron":
                        imgUrl = '../../Content/Images/visa.png';
                        break;
                    case "mastercard":
                        imgUrl = '../../Content/Images/master.png';
                        break;
                    case "discover":
                        imgUrl = '../../Content/Images/discover.jpg';
                        break;
                    case "maestro":
                        imgUrl = '../../Content/Images/maestro.png';
                        break;
                }
                $('#CN').css('background-image', 'url(' + imgUrl + ')');
            } else {
                $("#CN").css('background-image', 'none');
            }
        });
    });
}

function GetPlans(country) {
    $.ajax({
        url: '/Sales/GetPlansByCountries',
        type: "POST",
        cache: true,
        data: { 'country': country },
        success: function (data) {
            $('#Plans').empty();
            $("#plny").fadeOut("slow");
            $('#btnStep1').fadeOut("slow");
            if (data.length < 1) {
                alert('No existen planes para este país, por favor seleccione otro país e intente de nuevo');
            } else {
                $('#plny').html('<h5>Elige un plan</h5><select id="Plans" class="form-control"></select>');
                console.log("Planes");
                var opt1 = '<option value="0" disabled selected>Selecciona un plan</option>';
                $('#Plans').append(opt1);
                $(data).each(function (i, j) {
                    console.log(i, j);
                    opt1 = '<option value="' + j.PlanId + '" data-desc="' + j.Description + '" data-currency="' + j.Currency + '" ' +
                        'data-price="' + j.Value + '">' + j.PlanName + '</option>';
                    $('#Plans').append(opt1);
                });
                $("#plny").fadeIn("slow");
                $('#btnStep1').removeClass('hidden');
            }
            //HideCortinilla();
        },
        error: function (xhr) {
            debugger;
            alert('Hubo error en el tiempo de espera, por favor vuelva a intentarlo.');
            saleObj = {};

        }
    });
}

function SlideAndHide(id) {
    $(id).hide('fade', { direction: 'left' }, 300);
}

function GraphStep2() {
    $('#Stp2').fadeIn("slow");
}

function GraphStep3() {
    saleObj.PaymentType = 1;
    if (same) {
        $('#BuyerMail').val($('#Email').val());
        $('#BuyerName').val($('#Name').val() + " " + $('#LastName').val());
        if ($('#Mobile').val().length > 0) {
            $('#BuyerPhone').val($('#Mobile').val());
            $('.lblBBna').addClass("active");
        }
        $('.lblBBn').addClass("active");
    }
    FillReview();
    $('#Stp3').fadeIn("slow");
}

function GoBuy() {
    $('#ContenedorWarning').empty();
    $.ajax({
        url: '/Sales/SetSale',
        type: "POST",
        cache: true,
        data: { 'sale': saleObj },
        success: function (data) {
            var title;
            var content;
            var btn;
            data = $lf.parseString(data);
            if (data.Error.IsError) {
                title = 'Sale Error';
                var ds = data.Error.Description.split('-');
                content = '<label class="content-error">Description: ' + ds[0] + '<br>' +
                    'Message: ' + data.Error.Message + '<br>' +
                    'Source: ' + data.Error.Source + '</label>';
                btn = '<input type="submit" class="button-error btn" value="Review Sale" onclick="ReviewSale();" />';
            } else {
                var venta = data.Annotations.split('&');
                if (venta[0] === "2") {
                    title = 'Thanks for your purchase.';
                    content = '<label class="content-success">The sale has been submmited.' +
                        'The buyer will get an email soon.';
                    btn = '<input type="submit" class="button-success btn" value="Aceptar" onclick="NewSale();" />';
                } else {
                    title = 'Thanks for your purchase.';
                    content = '<label class="content-success">Sale Code: ' + venta[0] + '<br>' +
                        'Beginning date: ' + venta[1] + '<br>' +
                        'Ending date: ' + venta[2] + '</label>';
                    btn = '<input type="submit" class="button-success btn" value="Accept" onclick="NewSale();" />';
                }
            }
            BuildModal(title, content, btn);
        },
        error: function (xhr) {
            debugger;
            alert('Hubo error en el tiempo de espera, por favor vuelva a intentarlo.');
            saleObj = {};
        }
    });
}

function ReviewSale() {
    $(".atr3").removeClass("active ls-active");
    $(".atr3").removeClass("ls-past");
    $(".atr1").addClass("active ls-active");
    $("#Stp3").removeClass("active in");
    $("#Stp1").addClass("active in");
    $(".bar").css("width", "33.33333%");
}

function ReviewSaleR() {

}

function NewSale() {
    saleObj = {};
    location.reload();
}

function NewSaleR() {
    //window.location.href = "http://www.smilego.com/";
    window.location.href = "/";
}

function FillReview() {
    var pln = saleObj.PlanText.split('-');
    //var pln = $('#Plans').find(":selected")[0];    
    $(".buy-review").empty();
    $(".buy-review").append('<h5 class="section-title">Revisión de la compra</h5>');
    var ctn = '<div class="col-md-6">' +
        '<label class="title-review">Titular del plan</label>' +
        '<ul>' +
        '<li>Nombre: ' + $('#Name').val() + ' ' + $('#LastName').val() + '</li>' +
        '<li>Identificación: ' + $('#cId').val() + '</li>' +
        '<li>Celular: ' + $('#Mobile').val() + '</li>' +
        '<li>Email: ' + $('#Email').val() + '</li>' +
        //'<li>Género: ' + gnrr + '</li>' +
        '<li>Dirección: ' + $('#Address').val() + '</li>' +
        '<li>Fecha de nacimiento: ' + $('#Birthday').val() + '</li>' +
        '</ul>' +
        '</div>' +
        '<div class="col-md-4">' +
        '<label class="title-review">Plan Adquirido</label>' +
        '<ul>' +
        '<li>Nombre: ' + pln[0] + '</li>' +
        //'<li>Descripción: ' + pln.attributes[1].value + '</li>' +
        //'<li>País de uso: ' + $("#sltCountry option:selected").text() + '</li>' +
        '</ul>' +
        '</div>' +
        '<div class="col-md-2">' +
        '<label class="title-review">Total</label>' +
        '<span class="total-review">' + pln[1] + '</span>' +
        '</div>';
    $(".buy-review").append(ctn);
}

function BenScreen() {
    $('#Fillbens').append('<div class="col-md-12 benito"><h5 class="section-title">Ingresa los datos del beneficiario que usará el plan, los campos con (*) son obligatorios</h5>' +
        '<div class="form-group col-md-6">' +
        '<label for="Name' + existingBens + '" class="col-lg-4 control-label">Nombre (*)</label>' +
        '<div class="col-lg-8">' +
        '<input id="Name' + existingBens + '" type="text" class="selectium form-control">' +
        '</div>' +
        '</div>' +
        ' <div class="form-group col-md-6">' +
        '<label for="LastName' + existingBens + '" class="col-lg-4 control-label">Apellido (*)</label>' +
        '<div class="col-lg-8">' +
        '<input id="LastName' + existingBens + '" type="text" class="selectium form-control">' +
        '</div>' +
        '</div>' +
        '<div class="form-group col-md-6">' +
        '<label for="cId' + existingBens + '" class="col-lg-4 control-label">Identificación (*)</label>' +
        '<div class="col-lg-8">' +
        '<input id="cId' + existingBens + '" type="text" class="selectium form-control">' +
        '</div>' +
        '</div></div>');
    if ((max - $('.benito').length) !== 0) {
        $('.incld').empty();
        $('.incld').text('* Incluir beneficiarios (' + (max - 1) + ')');
    } else $('.incld').hide();
    existingBens++;
}

function GetCurrentPlan(a, b, c) {
    debugger;
}
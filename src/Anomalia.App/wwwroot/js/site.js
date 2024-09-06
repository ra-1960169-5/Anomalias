let show = true;
function Toggle() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle("active", show); 
    show = !show;
}


function DataPicker() {

    $(document).ready(function () {

        $('#datepicker').datepicker(
            {
                format: "mm/yyyy",
                viewMode: "months",
                minViewMode: "months",
            }

        );

        $('#datepickerDay').datepicker(
            {
                format: "dd/mm/yyyy",
            
            }

        );

        $('#datepickerDayFinal').datepicker(
            {
                format: "dd/mm/yyyy",
            }

        );


    });

    $.fn.datepicker.dates['en'] = {
        days: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sabado"],
        daysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sab"],
        daysMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sab"],
        months: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        today: "Hoje",
        clear: "Limpar",
        format: "dd/mm/yyyy",
        titleFormat: "MM yyyy",
        weekStart: 0
    };

}

function Modal() {
    $(function () {
            $.ajaxSetup({ cache: false });
            $("a[data-model]").on("click",
                function () {
                    $('#myModalContent').load(this.href,
                        function (responseTxt, statusTxt, xhr) {
                            if (statusTxt == "error") {
                                location.reload();
                            } else {
                                $('#myModal').modal({
                                    keyboard: true
                                }, 'show');
                                bindForm(this);
                            }
                        }
                    );

                    return false;
                }
            );
    });
   
    function bindForm(dialog) {
        $('form', dialog).submit(function () {

            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#myModal').modal('hide');
                        location.reload();
                    } else {
                        if (($('form', dialog).isValid)) {
                            $('#myModal').modal('hide');
                            location.reload();
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                }
            });
            return false;
        });
    }


}


 document.addEventListener("DOMContentLoaded", function (event) {
        const linkColor = document.querySelectorAll('.sidebar ul li')
        function colorLink() {
            if (linkColor) {
                linkColor.forEach(l => l.classList.remove('active'))
                this.classList.add('active')
            }
        }
        linkColor.forEach(l => l.addEventListener('click', colorLink));
});



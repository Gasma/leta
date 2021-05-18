function validar(nomeForm) {
    var aux = true;
    $(nomeForm).find('select, input').each(function () {
        if ($(this).prop('required') && (($(this).val() == "") || ($(this).val() == null))) {
            bloqueia($(this));
            aux = false;
            return false;
        }
    });
    return aux;
}
function bloqueia(campo) {
    campo.attr('data-toggle', 'tooltip')
        .attr('title', 'Preencha este campo.')
        .tooltip({
            trigger: 'manual'
        })
        .tooltip('show');
    setTimeout(function () {
        campo.tooltip('hide');
    }, 1500);
}
var weekdays = ["Domingo", "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado"];
$("#HoraDoDia").on("change", function () {
    var data = new Date($("#HoraDoDia").val());
    $("#DiaDaSemana").val(weekdays[data.getDay()]);
});

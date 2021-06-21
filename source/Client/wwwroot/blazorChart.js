window.updateChart = (chartName, title, labels, values) => {
    const data = {
        labels: labels,
        datasets: [{
            label: title,
            backgroundColor: 'rgb(255, 99, 132)',
            borderColor: 'rgb(255, 99, 132)',
            data: values,
        }]
    };
    const config = {
        type: 'line',
        data,
        options: {}
    };
    var ctx = document.getElementById(chartName);

    if (typeof myChart !== 'undefined') {
        myChart.destroy();
    }

    myChart = new Chart(
        ctx,
        config
    );
};
export function readableDate(postDate: string) {
    var year = new Date(postDate).getFullYear()
    var month = new Date(postDate).getMonth();
    var day = new Date(postDate).getDate();

    var hours = new Date(postDate).getHours();
    var minutes = new Date(postDate).getMinutes();

    var formattedDay = day < 10 ? '0' + day : day;
    var formattedMonth = month < 10 ? '0' + month : month;
    var formattedHours = hours < 10 ? '0' + hours : hours;
    var formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
    
    return formattedDay + "-" + formattedMonth + "-" + year + " " 
            + formattedHours + ":" + formattedMinutes;
}
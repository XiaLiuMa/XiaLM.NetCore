window.mypage = {
    table: "div",
    bar: "bar",
    limit: "10",
    color: "#1E9FFF",
    layout: ["count", "prev", "page", "next", "limit", "skip"],
    setCfg: function (b) {
        mypage.table = b.table;
        mypage.bar = b.bar;
        mypage.limit = b.limit;
        mypage.color = b.color;
        mypage.layout = b.layout;
    },
    returnHtml: function (g, e) {
        var h = '<table class="layui-table" lay-size="sm"><colgroup>';
        for (var f in e) {
            h += " <col width=" + e[f] + ">";
        }
        h += " </colgroup><thead><tr>";
        for (var f in g) {
            h += "  <th>" + g[f] + "</th>";
        }
        h += " </tr></thead> <tbody>";
        return h;
    },
    returnList: function (j) {
        var h = new Array();
        for (var f in j) {
            var i = "";
            for (var g in j[f]) {
                i += j[f][g] + "~";
            }
            i = i.substring(0, i.length - 1);
            h.push(i);
        }
        return h;
    },
    returnTable: function (e, i) {
        var h = e.split("~");
        var g = "<tr>";
        g += "<td><input type='checkbox' name='checktr' class='trcheck'></td>";
        g += "<td>" + i + "</td>"
        for (var f in h) {
            g += "<td>" + h[f] + "</td>";
        }
        g += "</tr>";
        return g;
    }
};
$(function () {
    layui.use("laypage", function () {
        var a = layui.laypage;
        a.render({
            elem: mypage.bar,
            limit: mypage.limit,
            theme: mypage.color,
            count: json.length,
            layout: mypage.layout,
            jump: function (b) {
                document.getElementById(mypage.table).innerHTML = (function () {
                    var c = [mypage.returnHtml(nameList, widthList)],
                      d = mypage
                        .returnList(json)
                        .concat()
                        .splice(b.curr * b.limit - b.limit, b.limit);
                    layui.each(d, function (e, g) {
                        var f = mypage.returnTable(g, ++e);
                        c.push(f);
                    });
                    c.push(" </tbody></table></br>");
                    return c.join("");
                })();
            }
        });
    });
});

// ��ʼ�����
function UpdataPage() {
    mypage.setCfg({
        table: "table",
        bar: "pageBar",
        limit: 10,   //����Ĭ��һҳ��ʾ����������
        color: "#1E9FFF",
        layout: ["count", "prev", "page", "next", "limit", "skip"]
    }); //��ʼ�����
}

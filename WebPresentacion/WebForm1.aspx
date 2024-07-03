<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebPresentacion.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/vis/4.21.0/vis.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/vis/4.21.0/vis.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <style>
        #mynetwork {
            width: 100%;
            height: 600px;
            border: 1px solid lightgray;
        }
    </style>

    <script>
        var nodes = new vis.DataSet();
        var edges = new vis.DataSet();
        var network = null;

        function drawGraph() {
            var container = document.getElementById('mynetwork');
            var data = {
                nodes: nodes,
                edges: edges
            };
            var options = {};
            network = new vis.Network(container, data, options);
        }

        function updateGraphWithData() {
            $.ajax({
                type: "POST",
                url: "WebForm1.aspx/GetGraphData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);

                    nodes.clear();
                    edges.clear();
                    nodes.add(data.nodes);
                    edges.add(data.edges);

                    // Actualizar la imagen base64 en el elemento img
                    var imgElement = document.getElementById('graphImage');
                    imgElement.src = "data:image/png;base64," + data.graphImage;

                    // Ajustar la vista del gráfico
                    network.fit();
                },
                error: function (error) {
                    console.error("Error al obtener datos del grafo: " + error.responseText);
                }
            });
        }


        $(document).ready(function () {
            updateGraphWithData(); // Actualiza el grafo con datos iniciales
            drawGraph(); // Dibuja el grafo después de actualizar los datos
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Agregar Nodo</h2>
            <asp:TextBox ID="TextBoxMatricula" runat="server" Placeholder="Matrícula"></asp:TextBox>
            <asp:CheckBox ID="CheckBoxAsistencia" runat="server" Text="Asistencia" />
            <asp:Button ID="ButtonAddNode" runat="server" Text="Agregar Nodo" OnClick="ButtonAddNode_Click" />

            <h2>Agregar Arco</h2>
            <asp:TextBox ID="TextBoxMatriculaOrigen" runat="server" Placeholder="Matrícula Origen"></asp:TextBox>
            <asp:TextBox ID="TextBoxMatriculaDestino" runat="server" Placeholder="Matrícula Destino"></asp:TextBox>
            <asp:TextBox ID="TextBoxPeso" runat="server" Placeholder="Peso"></asp:TextBox>
            <asp:Button ID="ButtonAddEdge" runat="server" Text="Agregar Arco" OnClick="ButtonAddEdge_Click" />

            <h2>Operaciones con el Grafo</h2>
            <asp:Button ID="ButtonTopologica" runat="server" Text="Buscar Topológica" OnClick="ButtonTopologica_Click" />
            <asp:Button ID="ButtonDijkstra" runat="server" Text="Aplicar Dijkstra" OnClick="ButtonDijkstra_Click" />
            <asp:Button ID="ButtonDFS" runat="server" Text="Recorrido DFS" OnClick="ButtonDFS_Click" />
            <asp:Button ID="ButtonBFS" runat="server" Text="Recorrido BFS" OnClick="ButtonBFS_Click" />

            <h3>Resultados</h3>
            <asp:Label ID="LabelResultado" runat="server"></asp:Label>

            <h2>Visualizar Grafo</h2>
            <asp:Button ID="ButtonVisualize" runat="server" Text="Visualizar Grafo" OnClientClick="updateGraphWithData(); return false;" />
            <div id="mynetwork"></div>
            <img id="graphImage" src="" alt="Graph Image" />
        </div>
    </form>
</body>
</html>

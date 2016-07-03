<%@ Page Title="Home Page" Language="C#" MasterPageFile="./Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="www.BankBals.Service._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class='div-bread'> 
        <a href="/BankBals">Главная</a> &#8594; Service
    </div>


    <h3>Sources map</h3>
    <ul>
    <li><a href="Sources.aspx?FormID=101">Form 101</a></li>
    <li><a href="Sources.aspx?FormID=102">Form 102</a></li>
    <li><a href="Sources.aspx?FormID=123">Form 123</a></li>
    <li><a href="Sources.aspx?FormID=134">Form 134</a></li>
    <li><a href="Sources.aspx?FormID=135">Form 135</a></li>
    </ul>


    <h3>Aggregates Members List</h3>
    <ul>
        <li><a href="AggsList.aspx?AggID=-6">1-200</a></li>
        <li><a href="AggsList.aspx?AggID=-5">101-...</a></li>
        <li><a href="AggsList.aspx?AggID=-4">21-100</a></li>
        <li><a href="AggsList.aspx?AggID=-3">2-20</a></li>
        <li><a href="AggsList.aspx?AggID=-1">Все банки</a></li>
    </ul>
</asp:Content>

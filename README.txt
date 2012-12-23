Проект CassetteKernel для MS VS 2010
Библиотека предназначена для работы с кассетами
Сопряженный пакет: CassetteExtension

Особенности

Формат файла *.finfo, расположенного в корне кассеты

<?xml version="1.0" encoding="utf-8"?>
<finfo>
  <image>
    <small previewBase="120" qualityLevel="90" />
    <medium previewBase="800" qualityLevel="90" />
    <normal previewBase="1024" qualityLevel="90" />
  </image>
  <video>
    <medium videoBitrate="400K" audioBitrate="22050" rate="10" framesize="384x288" />
  </video>
  <docnameprefix [issystem="cassettename"]>[имя префикса, напр. имя проекта]</docnameprefix>
</finfo> 

Элемент docnameprefix задает префикс для имен документов. Имена документов формируются как конкатенация 
префикса, пробела и первой части имени файла. Префикс по умолчанию - пустой и пробел не ставится. 
Иначе префикс может задаваться явно в теле элемента или неявно, как имя кассеты.

 
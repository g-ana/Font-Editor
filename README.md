# Font-Editor

Arduino LCD Font editor

Arduino LCD font editor е Windows Forms апликација за креирање и уредување на знаци (фонтови) за приказ на мали црно/бели  LCD и OLED дисплеи или on-screen display.  Овозможува поддршка за повеќе (засега комплетно покриени 2 фамилии на дисплеи) различни дисплеи. Овозможува креирање и уредување на знаците кои би се прикажувале на ваквите дисплеи и нивно извезување во форма на изворна датотека за вградување во проекти за развој на вградиви системи (изворни C заглавни датотеки).

Апликацијата е двојазична (англиски / македонски) при што (скоро) сите пораки се сместени во ресурсни датотеки и лесно може да се додаде поддршка и за други јазици. Еден дел од функциите се сместени во одвоена библиотека и се користат во форма на DLL од страна на главната апликација.

Во главниот прозорец на апликацијата се прикажуваат знаците во нивната природна големина како бит мапи. Со двоен клик на некој од знаците истиoт се отвора во нов прозорец за уредување кој е resizeable. Кликнување на површината поставува пиксели од знакот (при што промените се веднаш видливи и во главниот прозор на апликацијата). Во менијата и на лентата со алатки се поставени и основни алатки за манипулација со сликата на знакот (бришење, инвертирање, поместување во сите 4 насоки, пресликување по хоризонтала и вертикала. Поддржани се и основните операции со clipboard при што знакот може да се пренесе или увезе од битмапа со произволна големина (при што автоматски се прави скалирање). Дел од овие алатки се достапни и во менијата и лентата со алатки на главниот прозор и работат на тековно селектираниот знак (прикажан со светла рамка). Поддржана е серијализација (се зачувува целото множество знаци во интерен бинарен формат) и извоз во текстуални датотеки за вклучување во развојни околини за програмирање на микроконтролери. 
Поддржани се (засега) две фамилии на дисплеи:
1.	LCD 16x2 / LCD 20x4 (Hitachi HD44780)
2.	Nokia 5110 (PCD8544)
Првиот (всушност 2 слични) се дисплеи за прикажување на текст (одвоени знаци) организирани во 2 реда по 16 знаци или 4 реда по 20 знаци. Овие дисплеи веќе ги имаат вградено основните латинични знаци и имаат можност да им се дефинираат до 8 кориснички дефинирани знаци. Матрицата на знаците кај овие дисплеи е 8 редици и 5 колони за еден знак.
Вториот е дисплеј кој се користел кај постарите мобилни телефони и е графички дисплеј со резолуција 84x48 и не поседува вградени знаци. Кај овој дисплеј знаците се организирани во матрици 7 редици по 5 колони.
Апликацијата извезува .h датотека со дефинирање на вредностите на пикселите на знаците организирани во поле од константни вредности кое најчесто се сместува во флеш (read-only) меморијата на микроконтролерот, начин на кој тоа го прават некои популарни библиотеки за приказ на овие дисплеи за Arduino платформата.
Во формата за извоз може да постават некои параметри за извоз на податоците (тип на дисплеј, патека до излезната датотека, дефиниција на полето (во C програмски јазик) во кое ќе бидат сместени податоците, како и бројниот систем во кој ќе бидат извезени.

Пример за излез во датотека FINKI.h:
const static byte customChar[][5] = {
{60,66,127,66,60},
{127,96,24,6,127},
{127,16,16,16,127},
{127,8,20,34,65},
{127,96,24,6,127},
{0,0,0,127,0},
{62,99,65,99,62},
{30,35,68,35,30},
};

Пример за примената на датотеката во Arduino околината:
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include "FINKI.h"

LiquidCrystal_I2C lcd(0x27, 16, 2);

void setup(){
  lcd.begin();   // iInit the LCD for 16 chars 2 lines
  lcd.backlight();   // Turn on the backligt (try lcd.noBaklight() to turn it off)
  lcd.setCursor(0,0); //First line
  lcd.print("I2C LCD Cust.ch.");

  for(int i = 0; i<8; i++)
    lcd.createChar(i, customChar[i]);

//  lcd.home();
  lcd.setCursor(0,1); //Second line
  for(int i = 0; i<8; i++)
    lcd.write(i);
  delay(5000);
}

void loop() { }

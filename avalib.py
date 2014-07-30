from Tkinter import Tk, Text, BOTH, W, N, E, S
from ttk import Frame, Button, Label, Style, Entry, Checkbutton
from Tkinter import *
import feedparser
import urllib2
import time
import datetime

indigo = '#1b0432'
cream = '#FFFFF0'

def num2binarray(dec_no,size):
   bool_array = []
   for item in reversed(list(bin(dec_no)[2:])):
      if item == '0':
         bool_array.append(False)
      elif item == '1':
         bool_array.append(True)
   while(len(bool_array)<size):
      bool_array.append(False)
   return bool_array

def num2array(dec_no,size):
   array = []
   for x in range(0,size):
      if x == dec_no:
         array.append(True)
      else:
         array.append(False)
   return array

class AVA:
   def __init__(self):
      self.weather = Weather()
      self.nyt_fp = NYT_RSS('http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml')
      self.cal = Calendar()
      self.update()      

   def update(self):
      self.cal.update()
      self.weather.update()
      self.rss_feed = self.nyt_fp.pop_post()

class Weather:

   def __init__(self):
      self.temp = 3200
      self.cond = 3200
      self.high = 3200
      self.low  = 3200
      self.url = 'http://weather.yahooapis.com/forecastrss?w=12776650&u=f'
      self.update()

   def check(self):
      try:
         urllib2.urlopen(self.url)
         return True         # URL Exist
      except ValueError, ex:
         return False        # URL not well formatted
      except urllib2.URLError, ex:
         return False        # URL don't seem to be alive

   def update(self):
      if self.check():
         raw_feed = feedparser.parse(self.url)
         if raw_feed.entries[0].summary != 'Weather Data not Available at the moment':
            self.temp =  raw_feed.entries[0].yweather_condition['temp']
            self.cond =  raw_feed.entries[0].yweather_condition['code']
            dow = datetime.date.today().weekday()
            dowstr = ['Mon','Tue','Wed','Thu','Fri','Sat','Sun']
            self.high = raw_feed.entries[0].summary.partition(dowstr[dow])[2].partition('High: ')[2].partition(' Low:')[0]
            self.low  = raw_feed.entries[0].summary.partition(dowstr[dow])[2].partition('Low: ')[2].partition('<br />')[0]
         else:
            print 'GET DUNKED NERD'

class NYT_RSS:

   def __init__(self,url):
      self.url = url
      self.update()

   def update(self):
      raw_feed = feedparser.parse(self.url)
      self.headlines = []
      self.stories = []
      for post in raw_feed.entries:
         if post.title.encode('ascii', 'ignore') != '':
            self.headlines.append(post.title.encode('ascii', 'ignore'))
            self.stories.append(post.description.split('<img')[0].encode('ascii', 'ignore'))

   def pop_post(self):
      if len(self.headlines) == 0:
         self.update()
      return self.headlines.pop(),self.stories.pop()

class Calendar:
   def __init__(self):
      self.date_dy = 'DD'
      self.date_mo = 'MM'
      self.date_yr = 'YYYY'
      self.date_str = 'DD.MM.YYYY'
      self.time_mn = 'mm'
      self.time_hr = 'hr'
      self.time_mn_bin = []
      self.time_hr_bin = []
      self.dow = []
      self.update()
   def update(self):
      self.dow = num2array(datetime.date.today().weekday(),7)
      self.date_dy = str(datetime.date.today().day)
      self.date_mo = str(datetime.date.today().month)
      self.date_yr = str(datetime.date.today().year)
      if len(self.date_dy) == 1:
         self.date_dy = '0' + self.date_dy
      if len(self.date_mo) ==1:
         self.date_mo ='0' + self.date_mo
      self.date_str =  self.date_dy+'.'+self.date_mo+'.'+self.date_yr
      self.time_hr = datetime.datetime.now().time().hour
      self.time_hr_bin = num2binarray(datetime.datetime.now().time().hour,5)
      self.time_mn = datetime.datetime.now().time().minute
      self.time_mn_bin = num2binarray(datetime.datetime.now().time().minute,6)

class UI(Frame):
   def __init__(self, parent):
      Frame.__init__(self, parent) 
      self.parent = parent
      self.parent.bind('q',self.exit)
      self.parent.config(cursor="none")
      self.parent.focus_set()
      self.ava = AVA()
      self.pack(fill=BOTH, expand=1)
      self.configure(background = indigo)
      self.init_clock()
      self.grid_columnconfigure(0,minsize=20)
      self.grid_rowconfigure(0,minsize=20)
      self.grid_rowconfigure(8,minsize=40)
   def init_clock(self):
      self.lbl_date = Label(self, text=self.ava.cal.date_str, fg=cream, bg=indigo,font=('Consolas',19))
      self.lbl_date.grid(column=1,row=6,columnspan=3)

      self.lbl_hr = Label(self,text=self.ava.cal.time_hr, fg=cream, bg=indigo,font=('Consolas',19))
      self.lbl_hr.grid(column=2,row=8)

      self.lbl_mn = Label(self,text=self.ava.cal.time_mn, fg=cream, bg=indigo,font=('Consolas',19))
      self.lbl_mn.grid(column=3,row=8)

      self.cnv_dow = Canvas(self,width=40,height=280)
      self.cnv_dow.grid(sticky=S,column=1,row=7,padx=4,rowspan=2)
      self.dow_rect_bin = []
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,2,40,42,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,42,40,82,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,82,40,122,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,122,40,162,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,162,40,202,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,202,40,242,fill=indigo,outline=indigo))
      self.dow_rect_bin.append(self.cnv_dow.create_rectangle(3,242,40,282,fill=indigo,outline=indigo))
      self.set_bin(self.cnv_dow,self.ava.cal.dow,self.dow_rect_bin)
      self.cnv_dow.create_text(21.5,22,fill=indigo,text='SUN',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,62,fill=indigo,text='SAT',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,102,fill=indigo,text='FRI',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,142,fill=indigo,text='THU',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,182,fill=indigo,text='WED',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,222,fill=indigo,text='TUE',font=('Consolas',16))
      self.cnv_dow.create_text(21.5,262,fill=indigo,text='MON',font=('Consolas',16))


      self.cnv_hr = Canvas(self,width=40,height=200)
      self.cnv_hr.grid(sticky=S,column=2,row=7,padx=4)
      self.hr_bin = []
      self.hr_bin.append(self.cnv_hr.create_rectangle(3,2,40,42,fill=indigo,outline=indigo))
      self.hr_bin.append(self.cnv_hr.create_rectangle(3,42,40,82,fill=indigo,outline=indigo))
      self.hr_bin.append(self.cnv_hr.create_rectangle(3,82,40,122,fill=indigo,outline=indigo))
      self.hr_bin.append(self.cnv_hr.create_rectangle(3,122,40,162,fill=indigo,outline=indigo))
      self.hr_bin.append(self.cnv_hr.create_rectangle(3,162,40,202,fill=indigo,outline=indigo))
      self.set_bin(self.cnv_hr,self.ava.cal.time_hr_bin,self.hr_bin)

      self.cnv_mn = Canvas(self,width=40,height=240)
      self.cnv_mn.grid(sticky=S,column=3,row=7,padx=4)
      self.mn_bin = []
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,2,40,42,fill=indigo,outline=indigo))
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,42,40,82,fill=indigo,outline=indigo))
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,82,40,122,fill=indigo,outline=indigo))
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,122,40,162,fill=indigo,outline=indigo))
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,162,40,202,fill=indigo,outline=indigo))
      self.mn_bin.append(self.cnv_mn.create_rectangle(3,202,40,242,fill=indigo,outline=indigo))
      self.set_bin(self.cnv_mn,self.ava.cal.time_mn_bin,self.mn_bin)
      self.after(1000,self.update_clock)

   def update_clock(self):
      self.ava.cal.update()
      self.set_bin(self.cnv_dow,self.ava.cal.dow,self.dow_rect_bin)
      self.set_bin(self.cnv_hr,self.ava.cal.time_hr_bin,self.hr_bin)
      self.set_bin(self.cnv_mn,self.ava.cal.time_mn_bin,self.mn_bin)
      self.lbl_hr.config(text=self.ava.cal.time_hr)
      self.lbl_mn.config(text=self.ava.cal.time_mn)
      self.lbl_date.config(text=self.ava.cal.date_str)
      self.after(1000,self.update_clock)




   def set_bin(self,cnv,ctrl_array,ui_array):
      # print ctrl_array
      # print ui_array
      for ctrl,ui in zip(ctrl_array,reversed(ui_array)):
         if ctrl==True:
            cnv.itemconfig(ui,fill=cream,outline=cream)
         else:
            cnv.itemconfig(ui,fill=indigo,outline=indigo)

      

   def exit(self,event):
      # query = repr(event.char)
      self.quit()

def main():
   root = Tk()
   # w, h = root.winfo_screenwidth(), root.winfo_screenheight()
   # root.overrideredirect(1)
   # root.geometry("%dx%d+0+0" % (w, h))
   w, h = root.winfo_screenwidth(), root.winfo_screenheight()
   root.geometry("%dx%d+0+0" % (w/2-16, h-79))
   app = UI(root)
   root.mainloop() 


if __name__ == '__main__':
   main()
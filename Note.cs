﻿/* 
		Снипеты
	ctor - создание конструктора
	
		nameof() - отдает имя передаваемой в неё переменной

            Первостепенно

  [-]  При остановке парсера останавливается программа. Так быть не должно. Даже если грохнется шедуллер и парсер - бот должен обрабатывать запросы пользователей
  [-]  Условие для парсера 
  [-]  При дебаге получается что статус выставляется, а стих скачаться не успел и при следующем проходе уже не возьмет его (Много "висящих" в процессе)
  [-]  Посмотреть на память в процессе работы программы. Возможно объекты не чистятся
            Второстепенно

  [-]  Переделать цвет иконки на более приятный!
  [-]  Добавить систему лайк - дизлайк?
  [-]  Добавить боту настройку (искать только в категории "love", теги)
  [-]  У лавки медвежонка крутой способ опталы через Юмани
  [-]  Как вызвать бота в чате с другим собеседником? (inline mode)
  [-]  Можно ли оставлять курсор вначале отправляемого сообщения
  [-]  Добавить возможность получения стиха не путем парсинга, а н-р по апишке
  [-]  Разделить бота, парсер и шедулер на 3 разных программы
  [-]  Пусть он мне присылает сообщение каждый раз когда подключился новый пользователь
  [-]  Посмотреть - а что происходит с объектами стиха при парсинге. Они удаляются или лежат в памяти?

* What is ?
NoNameClass noName = new NoNameClass(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/url_list.txt"));
await noName.UrlListToSQL();


* Полезные заметки
            // NOTE: Юзать поле если private 
            // NOTE: ctrl+r+r
            // NOTE: Методы расширения
            // В SQL Нет порядка!

/internal readonly static string connectionStringOld = "Server=127.0.0.1;Database=newdb;Uid=root;Pwd=qwerty;";

        /*
        /// Разбивает строку на части, кратные входному параметру
        internal static List<string> Split(string str, int chunkSize)
        {
            var res = new List<string>();
            
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                if ((i + chunkSize) <= str.Length) res.Add(str.Substring(i, chunkSize));
                else res.Add(str.Substring(i, str.Length - i)); // Если оставшаяеся часть меньше chunksize - добавляем её
            }

            return res;
        } 
        */
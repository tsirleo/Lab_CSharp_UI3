# Лабораторная работа "Архитектура Model-View-ViewModel" 

Требуется переписать [вторую лабораторную работу](https://github.com/tsirleo/Lab_CSharp_UI2) в соответствии с архитектурой **MVVM (Model-View-ViewModel)**.

### Требования:
1. В решение должны входить пять проектов: **Model, ModelTests, ViewModel, View, ViewModelTests**. Названия проектов вы можете выбирать по своему усмотрению. Проект **View** не может ссылаться (***project reference***) на проект **Model**.

2. В проекте **ModelTests** должны находиться несколько осмысленных unit-тестов на различные аспекты основных вычислительных алгоритмов.

3. В проекте **ViewModelTests** должны находиться несколько осмысленных unit-тестов наразличные аспекты **ViewModel**.

4. Классы **Model** не должны иметь участвовать в привязках (***binding***) к пользовательскому интерфейсу.

5. Классы **Model** и **ViewModel** не должны напрямую обращаться к пользовательскому интерфейсу, в том числе использовать **MessageBox**, диалоговые окна и прочие компоненты пользовательского интерфейса.

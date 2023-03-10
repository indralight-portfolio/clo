# 필수 요구 사항
- csv 혹은 json 파일 업로드시 작동
- request body 에 csv 혹은 json 직접 입력시 작동

# 기타 사항
- Persistence layer 는 EF Core 6 사용
- DBMS 는 mysql 8.0 사용
- Paging 은 XPagedList 사용
- 동명이인은 없다고 가정. name 을 PK 로 하여 데이터가 존재하면 update 없으면 insert 처리

# DB 테이블 스키마
```sql
create table clo.`Employee`
(
   `Name`      varchar(20)
                 character set utf8mb4
                 collate utf8mb4_0900_ai_ci
                 not null,
   `Email`     varchar(100)
                 character set utf8mb4
                 collate utf8mb4_0900_ai_ci
                 null,
   `Tel`       varchar(20)
                 character set utf8mb4
                 collate utf8mb4_0900_ai_ci
                 null,
   `Joined`    datetime(3) null,
   primary key(`Name`)
)
engine innodb
collate 'utf8mb4_0900_ai_ci'
row_format default
```

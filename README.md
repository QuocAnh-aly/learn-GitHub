Các lệnh GitHubØ
-- sau đây tôi sẽ bí kíp học github trong 30p nhé.

1. tạo tài khoản github trên máy local

git config --global user.name newname // đổi tên người dùng
git config --global user.email newmail@domain.com // đổi email

2. Khỏi tạo một Local Repository mới trên máy của mình (máy local)

git init

3. Kiểm tra trạng thái của Repo

git status

// staging area hiểu đơn giản là khu vực để theo giỏi các file thực hiện commit tiếp theo 4. đẩy dữ liệu vào taged:

git add newfile

4.1 quay lại làm việc

git restore --staged namefile

5. đẩy dữ liệu từ taged vào commit mới

git commit -m " thông báo .. "

6. xem lại commit đã tạo

git log

// bây giờ chúng ta làm việc với server nhé (github)

7. Kết nối repo trên local với repo trên github:

git remote add origin "đường dân với github của mình ..."

// làm việc với nhánh branch

8. liệt kê các nhánh

git branch

9. tạo nhánh mới, khi đang đứng ở một snapshpt cũ

git checkout -b branchname

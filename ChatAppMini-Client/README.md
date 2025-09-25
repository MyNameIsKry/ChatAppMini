# ChatAppMini - Client

Ứng dụng chat realtime được xây dựng với Next.js 15, TypeScript, và SignalR.

## Tính năng

- 🔐 **Xác thực người dùng**: Đăng ký, đăng nhập với JWT
- 💬 **Chat realtime**: Sử dụng SignalR để nhắn tin thời gian thực
- 👥 **Chat nhóm**: Tạo và tham gia các cuộc trò chuyện nhóm
- 🔍 **Tìm kiếm người dùng**: Tìm và thêm bạn bè vào cuộc trò chuyện
- 📱 **Giao diện responsive**: Tối ưu cho cả desktop và mobile
- 🎨 **UI hiện đại**: Sử dụng Tailwind CSS và shadcn/ui

## Công nghệ sử dụng

- **Frontend**: Next.js 15, React 19, TypeScript
- **UI Components**: shadcn/ui, Tailwind CSS
- **State Management**: Zustand
- **Form Handling**: React Hook Form + Zod
- **HTTP Client**: Axios
- **Real-time**: SignalR
- **Notifications**: Sonner

## Cài đặt

1. Clone repository
2. Cài đặt dependencies:
```bash
npm install
```

3. Tạo file `.env.local`:
```env
NEXT_PUBLIC_API_URL=https://localhost:7189/api
NEXT_PUBLIC_SIGNALR_URL=https://localhost:7189/chatHub
```

4. Chạy ứng dụng:
```bash
npm run dev
```

## Cấu trúc thư mục

```
src/
├── app/                    # App Router pages
│   ├── (main)/            # Protected routes
│   ├── chat/              # Chat page
│   ├── login/             # Login page
│   ├── register/          # Register page
│   └── page.tsx           # Home page
├── components/            # React components
│   ├── ui/                # shadcn/ui components
│   └── providers/         # Context providers
├── hooks/                 # Custom hooks
├── lib/                   # Utilities and API
├── store/                 # Zustand stores
└── types/                 # TypeScript types
```

## Tính năng chính

### 1. Landing Page (/)
- Trang chủ giới thiệu ứng dụng
- Có thể truy cập bởi tất cả người dùng
- Liên kết đến trang đăng nhập/đăng ký

### 2. Authentication (/login, /register)
- Form đăng nhập và đăng ký với validation
- Tự động redirect nếu đã đăng nhập
- Lưu token vào cookies

### 3. Chat Page (/chat)
- Danh sách cuộc trò chuyện
- Tìm kiếm và thêm người dùng mới
- Chat realtime với SignalR
- Hiển thị tin nhắn theo thời gian thực

### 4. Profile Page (/profile)
- Xem và chỉnh sửa thông tin cá nhân
- Đăng xuất
- Quản lý tài khoản

## Chạy ứng dụng

```bash
npm run dev
# hoặc
yarn dev
# hoặc
pnpm dev
```

Mở [http://localhost:3000](http://localhost:3000) để xem ứng dụng.

## Deployment

1. Build ứng dụng:
```bash
npm run build
```

2. Start production server:
```bash
npm start
```

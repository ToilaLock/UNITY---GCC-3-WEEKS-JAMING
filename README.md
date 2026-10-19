# Boss Fight 2D – Minotaur

Game hành động 2D side-scrolling, người chơi đấu với boss Minotaur trong một đấu trường. Đây là project cá nhân làm trong sự kiện 3 tuần Game Jam nội bộ tại CLB Nhà Sáng Tạo Game PTIT (GCC)

## Tính năng

- Player: di chuyển, nhảy, đánh combo 3 đòn, dash, roll (có i-frames), block
- Boss Minotaur với AI nhiều trạng thái: đi theo người chơi, 2 đòn đánh thường, đòn đặc biệt (Start → Loop → Stop), phòng thủ, bị đánh, chết
- Boss có Phase 2 khi HP xuống dưới 50%
- Hệ thống Hitbox / Hurtbox, knockback, hit stop, camera shake
- UI: Main Menu, Pause, màn Victory / Defeat, thanh máu Player và Boss
- Âm thanh: SFX, lồng tiếng (voice) cho Player và Boss, nhạc nền, chỉnh âm lượng qua AudioMixer

## Điều khiển

| Phím | Hành động |
|---|---|
| `A` / `D` | Di chuyển trái / phải |
| `W` / `Space` | Nhảy |
| `Left Mouse` | Đánh thường (bấm liên tục để combo) |
| `E` | Đòn đặc biệt |
| `Shift` | Dash (giữ để dash dài hơn) |
| `E` | Roll |
| `Q` | Block |
| `Esc` | Pause |

Hỗ trợ cả Gamepad.

## Kiến thức áp dụng

### New Input System
- Input Actions asset với Action Map `Player` và `UI`
- Binding cho Keyboard và Gamepad, 2D Vector Composite, Hold Interaction (dash)
- Đọc input qua callback `started / performed / canceled`
- Chuyển Action Map khi pause hoặc menu

### State Machine (interface)
- Interface `IState` (`Enter`, `Tick`, `FixedTick`, `Exit`) và class `StateMachine`
- Dùng chung cho Player, Boss và GameManager
- Player states: Idle, Run, Jump, Fall, Attack, Dash, Roll, Block, Hurt, Dead
- Boss states: Idle, Chase, Attack1, Attack2, SpecialAttack, Defend, TakeHit, Dead
- Game states: Menu, Playing, Paused, Victory, GameOver

### Animator
- Animator Controller riêng cho Player và Boss
- Animation Event để bật/tắt hitbox và phát SFX đúng frame

### Âm thanh
- AudioMixer với các nhóm Master / SFX / Voice / Music
- AudioManager (Singleton, pooling AudioSource) và SoundData bằng ScriptableObject

### Khác
- ScriptableObject cho chỉ số (PlayerStats, BossStats)
- Event-driven (C# event / Action) cho Health và UI
- Cinemachine (camera shake), Tilemap, Particle System

## Cấu trúc thư mục

```
Updating..
```

## Credits

- **Boss Minotaur sprite**: [chierit](https://chierit.itch.io/boss-minotaur), giấy phép [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/)
- **Player sprite**: `Updating..`
- **SFX / Nhạc / Voice**: `Updating..`

> Asset của bên thứ ba thuộc về tác giả tương ứng và tuân theo giấy phép riêng của họ.

## Tác giả

`Đặng Đức Lộc` – `https://github.com/ToilaLock`
